using Cars411.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cars411.Controllers
{
    [FilterConfig.NoDirectAccess]
    [FilterConfig.AuthorizeActionFilter]
    public partial class PostsController : Controller
    {
        private Cars411Entities dbEntities = new Cars411Entities();
        public virtual ActionResult Index(string message)
        {
            try
            {
                int UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag.message = message;
                List<Posts> post = new List<Posts>();
                if (Convert.ToString(Session["UserType"]) == "Admin")
                {
                    var query = (from p in dbEntities.tblPosts
                                 join make in dbEntities.tblMakes on p.MakeID equals make.MakeID
                                 //join model in dbEntities.tblModels on new { p.ModelID, p.MakeID } equals new { model.ModelID, model.MakeId }
                                 join model in dbEntities.tblModels on new { p.ModelID} equals new { model.ModelID}
                                 join year in dbEntities.tblYears on p.YearID equals year.YearID
                                 select new
                                 {
                                     p.PostID,
                                     p.PostImage,
                                     p.PostTitle,
                                     p.ServiceCost,
                                     make.Maketitle,
                                     model.Modeltitle,
                                     year.Yeartitle,
                                     p.PublishBy
                                 }).ToList();
                    foreach (var item in query)
                    {
                        post.Add(new Posts()
                        {
                            PostID = item.PostID,
                            PostImage = item.PostImage,
                            PostTitle = item.PostTitle,
                            ServiceCost = item.ServiceCost,
                            Maketitle = item.Maketitle,
                            Modeltitle = item.Modeltitle,
                            Yeartitle = item.Yeartitle,
                            PublishBy = item.PublishBy
                        });
                    }
                    ViewBag.PostList = post;
                }
                else
                {
                    var query = (from p in dbEntities.tblPosts
                                 join make in dbEntities.tblMakes on p.MakeID equals make.MakeID
                                 join model in dbEntities.tblModels on p.ModelID equals model.ModelID
                                 join year in dbEntities.tblYears on p.YearID equals year.YearID
                                 where p.PostedBy == UserID
                                 select new
                                 {
                                     p.PostID,
                                     p.PostImage,
                                     p.PostTitle,
                                     p.ServiceCost,
                                     make.Maketitle,
                                     model.Modeltitle,
                                     year.Yeartitle,
                                     p.PublishBy
                                 }).ToList();
                    foreach (var item in query)
                    {
                        post.Add(new Posts()
                        {
                            PostID = item.PostID,
                            PostImage = item.PostImage,
                            PostTitle = item.PostTitle,
                            ServiceCost = item.ServiceCost,
                            Maketitle = item.Maketitle,
                            Modeltitle = item.Modeltitle,
                            Yeartitle = item.Yeartitle,
                            PublishBy = item.PublishBy
                        });
                    }
                    ViewBag.PostList = post;
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public virtual ActionResult Create()
        {
            try
            {
                ViewBag.Makes = new ModelHelper().ToSelectItemList(dbEntities.tblMakes, 1).ToList();
                ViewBag.Model = new ModelHelper().ToSelectItemList(dbEntities.tblModels, 2).ToList();
                ViewBag.Year = new ModelHelper().ToSelectItemList(dbEntities.tblYears, 3).ToList();
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public virtual ActionResult Create([Bind(Include = "PostID,IsActive, PostTitle,PostDescription,MakeID,ModelID,YearID,PostImage,ServiceCost,Relaibility,Comfort,Safety")] tblPost post, HttpPostedFileBase Image)
        {
            try
            {
                if (post.Relaibility == null)
                {
                    post.Relaibility = 0;
                }
                if (post.Safety == null)
                {
                    post.Safety = 0;
                }
                if (post.Comfort == null)
                {
                    post.Comfort = 0;
                }
                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg" };

                if (Image != null)
                {
                    var ext = Path.GetExtension(Image.FileName);
                    string myfile = post.PostTitle + "_" + post.PostID + ext;
                    var path = Path.Combine(Server.MapPath("~/PostImages"), myfile);
                    var path1 = Path.Combine(("\\PostImages"), myfile);
                    post.PostImage = path1;
                    Image.SaveAs(path);
                }
                else
                {
                    post.PostImage = "\\PostImages\\placeholder.jpg";
                }
                post.PostedDate = DateTime.Now;
                post.PostedBy = Convert.ToInt32(Session["UserID"]);
                post.PublishBy = Convert.ToString(Session["UserType"]);
                dbEntities.tblPosts.Add(post);
                dbEntities.SaveChanges();
                int postID = post.PostID;
                tblComment comment = new tblComment();
                comment.IsParent = 0;
                comment.ParentID = 0;
                comment.Comfort = post.Comfort;
                comment.Safety = post.Safety;
                comment.Relaiability = post.Relaibility;
                comment.PostID = postID;
                dbEntities.tblComments.Add(comment);
                dbEntities.SaveChanges();
                { return RedirectToAction("Index", new { message = "Post publish successfully." }); }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public virtual ActionResult Edit(int? id)
        {
            try
            {
                tblPost post = dbEntities.tblPosts.Where(x => x.PostID == id).FirstOrDefault();
                ViewBag.Makes = new ModelHelper().ToSelectItemList(dbEntities.tblMakes, 1).ToList();
                ViewBag.Model = new ModelHelper().ToSelectItemList(dbEntities.tblModels, 2).ToList();
                ViewBag.Year = new ModelHelper().ToSelectItemList(dbEntities.tblYears, 3).ToList();
                return View(post);

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public virtual ActionResult Edit([Bind(Include = "PostID,IsActive, PostTitle,PostDescription,MakeID,ModelID,YearID,PostImage,ServiceCost,Relaibility,Comfort,Safety,PostedBy,PublishBy")] tblPost post, HttpPostedFileBase Image)
        {
            try
            {
                if (post.Relaibility == null)
                {
                    post.Relaibility = 0;
                }
                if (post.Safety == null)
                {
                    post.Safety = 0;
                }
                if (post.Comfort == null)
                {
                    post.Comfort = 0;
                }
                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg" };

                if (Image != null)
                {
                    var ext = Path.GetExtension(Image.FileName);
                    string myfile = post.PostTitle + "_" + post.PostID + ext;
                    var path = Path.Combine(Server.MapPath("~/PostImages"), myfile);
                    var path1 = Path.Combine(("\\PostImages"), myfile);
                    post.PostImage = path1;
                    Image.SaveAs(path);
                }
                post.PostedDate = DateTime.Now;

                dbEntities.Entry(post).State = EntityState.Modified;
                dbEntities.SaveChanges();

                tblComment comment = dbEntities.tblComments.Where(x => x.PostID == post.PostID && x.ParentID == 0 && x.IsParent == 0).FirstOrDefault();
                comment.Comfort = post.Comfort;
                comment.Safety = post.Safety;
                comment.Relaiability = post.Relaibility;
                dbEntities.Entry(comment).State = EntityState.Modified;
                dbEntities.SaveChanges();
                { return RedirectToAction("Index", new { message = "Post publish successfully." }); }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeletePost(int? id)
        {
            try
            {
                List<tblComment> comment = dbEntities.tblComments.Where(x => x.PostID == id).ToList();
                foreach (var item in comment)
                {
                    int com = Convert.ToInt32(item.CommentID);
                    tblComment comment1 = dbEntities.tblComments.Find(com);
                    dbEntities.tblComments.Remove(comment1);
                    dbEntities.SaveChanges();

                }
                tblPost post = dbEntities.tblPosts.Find(id);
                if (post != null)
                {
                    dbEntities.tblPosts.Remove(post);
                    dbEntities.SaveChanges();


                }
                return RedirectToAction("Index", new { message = "Post deleted successfully" });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}