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
    public partial class BlogsController : Controller
    {
        private Cars411Entities dbEntities = new Cars411Entities();

        public virtual ActionResult Details(string message = null)
        {
            try
            {
                ViewBag.message = message;
                ViewBag.BlogList = dbEntities.tblBlogs.ToList();
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Create([Bind(Include = "BlogID, Blogtitle,Description,BlogImage")] tblBlog blog, HttpPostedFileBase Image)
        {
            try
            {
                if (Image != null)
                {
                    var ext = Path.GetExtension(Image.FileName);
                    string myfile = blog.Blogtitle + "_" + blog.BlogID + ext;
                    var path = Path.Combine(Server.MapPath("~/BlogImages"), myfile);
                    var path1 = Path.Combine(("\\BlogImages"), myfile);
                    blog.BlogImage = path1;
                    Image.SaveAs(path);
                }
                else
                {
                    blog.BlogImage = "\\BlogImages\\placeholder.jpg";
                }
                blog.IsActive = true;
                blog.CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                dbEntities.tblBlogs.Add(blog);
                dbEntities.SaveChanges();
                return RedirectToAction("Details", new { message = "Blog add successfully" });
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
                tblBlog blog = dbEntities.tblBlogs.Where(x => x.BlogID == id).FirstOrDefault();
                return View(blog);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public virtual ActionResult Edit([Bind(Include = "BlogID, Blogtitle,Description,BlogImage")] tblBlog blog, HttpPostedFileBase Image)
        {
            try
            {
                if (Image != null)
                {
                    var ext = Path.GetExtension(Image.FileName);
                    string myfile = blog.Blogtitle + "_" + blog.BlogID + ext;
                    var path = Path.Combine(Server.MapPath("~/BlogImages"), myfile);
                    var path1 = Path.Combine(("\\BlogImages"), myfile);
                    blog.BlogImage = path1;
                    Image.SaveAs(path);
                }

                blog.IsActive = true;
                blog.CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                dbEntities.Entry(blog).State = EntityState.Modified;
                dbEntities.SaveChanges();
                return RedirectToAction("Details", new { message = "Blog edit successfully" });
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteBlog(int? id)
        {
            try
            {
                tblBlog blog = dbEntities.tblBlogs.Find(id);
                if (blog != null)
                {
                    dbEntities.tblBlogs.Remove(blog);
                    dbEntities.SaveChanges();
                }
                return RedirectToAction("Details", new { message = "Blog deleted successfully" });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}