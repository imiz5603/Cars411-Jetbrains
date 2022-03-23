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
    public partial class UserController : Controller
    {
        private Cars411Entities dbEntities = new Cars411Entities();
        public virtual ActionResult Index(string message)
        {
            try
            {
                ViewBag.message = message;
                ViewBag.UserList = dbEntities.tblUsers.ToList();
                return View();

            }
            catch (Exception)
            {

                throw;
            }

        }
        public virtual ActionResult UserLogin(int? id)
        {

            return View();
        }
        public virtual ActionResult CreateUser()
        {
            return View();
        }
        public virtual ActionResult EditUser(int? id)
        {
            try
            {
                ViewBag.message = null;
                tblUser User = dbEntities.tblUsers.Where(x => x.User_ID == id).FirstOrDefault();
                ViewBag.LicenseExpiry = Convert.ToDateTime(User.LicenseExpiry).ToString("yyyy-MM-dd");
                ViewBag.ServiceExpiry = Convert.ToDateTime(User.ServiceExpiry).ToString("yyyy-MM-dd");
                return View(User);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public virtual ActionResult EditUser([Bind(Include = "User_ID,IsActive, Password,Email_Adress,UserName,MobileNo,LicenseExpiry,ServiceExpiry,ImagePath,Access_Level_ID,UserType")] tblUser user, HttpPostedFileBase Image)
        {
            try
            {

                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg" };

                if (Image != null)
                {
                    var ext = Path.GetExtension(Image.FileName);
                    string myfile = user.UserName + "_" + user.User_ID + ext;
                    var path = Path.Combine(Server.MapPath("~/UserImages"), myfile);
                    var path1 = Path.Combine(("\\UserImages"), myfile);
                    user.ImagePath = path1;
                    Image.SaveAs(path);
                }
                user.Create_Date = DateTime.Now;
                if (user.Access_Level_ID == 1)
                {
                    user.UserType = "Admin";
                }
                if (user.Access_Level_ID == 2)
                {
                    user.UserType = "User";
                }
                if (user.Access_Level_ID == 3)
                {
                    user.UserType = "Service Master";
                }
                dbEntities.Entry(user).State = EntityState.Modified;
                dbEntities.SaveChanges();
                { return RedirectToAction("Index", new { message = "User Profile successfully updated." }); }

            }
            catch (Exception)
            {

                throw;
            }


        }
        [HttpPost]
        public virtual ActionResult CreateUser([Bind(Include = "UserID,IsActive, Password,Email_Adress,UserName,MobileNo,LicenseExpiry,ServiceExpiry,Access_Level_ID")] tblUser user, HttpPostedFileBase Image)
        {
            try
            {
                tblUser User = dbEntities.tblUsers.Where(x => x.Email_Adress == user.Email_Adress).FirstOrDefault();
                if (User == null)
                {
                    if (Image != null)
                    {
                        var ext = Path.GetExtension(Image.FileName);
                        string myfile = user.UserName + "_" + user.User_ID + ext;
                        var path = Path.Combine(Server.MapPath("~/UserImages"), myfile);
                        var path1 = Path.Combine(("\\UserImages"), myfile);
                        user.ImagePath = path1;
                        Image.SaveAs(path);
                    }
                    else
                    {
                        user.ImagePath = "\\UserImages\\user.jpg";
                    }
                    if (user.Access_Level_ID == 1)
                    {
                        user.UserType = "Admin";
                    }
                    if (user.Access_Level_ID == 2)
                    {
                        user.UserType = "User";
                    }
                    if (user.Access_Level_ID == 3)
                    {
                        user.UserType = "Service Master";
                    }
                    user.Create_Date = DateTime.Now;
                    dbEntities.tblUsers.Add(user);
                    int rows = dbEntities.SaveChanges();
                    if (rows > 0)
                    { return RedirectToAction("Index", new { message = "User successfully registered." }); }
                }
                else
                {
                    ViewBag.message = "Supplied email already registered";
                    return View();
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public virtual ActionResult ProfileSetting()
        {
            try
            {
                int UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag.message = null;
                tblUser User = dbEntities.tblUsers.Where(x => x.User_ID == UserID).FirstOrDefault();
                ViewBag.LicenseExpiry = Convert.ToDateTime(User.LicenseExpiry).ToString("yyyy-MM-dd");
                ViewBag.ServiceExpiry = Convert.ToDateTime(User.ServiceExpiry).ToString("yyyy-MM-dd");
                return View(User);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public virtual ActionResult ProfileSetting([Bind(Include = "User_ID, Password,Email_Adress,UserName,MobileNo,LicenseExpiry,ServiceExpiry,ImagePath,Access_Level_ID,UserType")] tblUser user, HttpPostedFileBase Image)
        {
            try
            {

                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg" };
                if (Image != null)
                {
                    var ext = Path.GetExtension(Image.FileName);
                    string myfile = user.UserName + "_" + user.User_ID + ext;
                    var path = Path.Combine(Server.MapPath("~/UserImages"), myfile);
                    var path1 = Path.Combine(("\\UserImages"), myfile);
                    user.ImagePath = path1;
                    Image.SaveAs(path);
                }
                user.IsActive = true;
                user.Create_Date = DateTime.Now;
                dbEntities.Entry(user).State = EntityState.Modified;
                dbEntities.SaveChanges();
                ViewBag.message = "Profile successfully updated";
                tblUser tblUser = dbEntities.tblUsers.Where(x => x.User_ID == user.User_ID).FirstOrDefault();
                ViewBag.LicenseExpiry = Convert.ToDateTime(tblUser.LicenseExpiry).ToString("yyyy-MM-dd");
                ViewBag.ServiceExpiry = Convert.ToDateTime(tblUser.ServiceExpiry).ToString("yyyy-MM-dd");
                Session["UserName"] = tblUser.UserName;
                Session["ImagePath"] = tblUser.ImagePath;
                return View(tblUser);

            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}