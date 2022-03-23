using Cars411.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Cars411.Controllers
{

    public partial class AccountController : Controller
    {

        private Cars411Entities dbEntities = new Cars411Entities();
        public virtual ActionResult Login()
        {
            Session["UserType"] = null;//test
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session["ImagePath"] = null;
            Session["Menu"] = null;
            return View();
        }



        [HttpPost]
        public virtual ActionResult Login(string Email, string password)
        {
            try
            {
                tblUser User = dbEntities.tblUsers.Where(x => x.Email_Adress == Email && x.Password == password).FirstOrDefault();
                if (User != null)
                {
                    if (User.IsActive == true)
                    {
                        Session["UserType"] = User.UserType;
                        Session["UserID"] = User.User_ID;
                        Session["UserName"] = User.UserName;
                        Session["Email"] = User.Email_Adress;
                        Session["ImagePath"] = User.ImagePath;
                        Session["Menu"] = dbEntities.getAccessMenusUser(User.User_ID).ToList();
                        return RedirectToAction("Userlogin", "User", new { id = User.User_ID });
                    }
                    else if (User.IsActive == false)
                    {
                        ViewBag.IsError = "Your Account is deactivated please contact to Admin";
                        return View();
                    }
                }
                else
                {
                    ViewBag.IsError = "Your Credentials are incorrect";
                    return View();
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }


        }
        public virtual ActionResult Changepassword()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Changepassword(string oldPassword, string newPassword)
        {
            try
            {
                string Email = Session["Email"].ToString();
                tblUser User = dbEntities.tblUsers.Where(x => x.Password == oldPassword && x.Email_Adress == Email).FirstOrDefault();
                if (User == null)
                {
                    ViewBag.IsError = "Your Old Password is incorrect";
                    return View();
                }
                else
                {
                    User.Password = newPassword;
                    dbEntities.Entry(User).State = EntityState.Modified;
                    dbEntities.SaveChanges();
                    ViewBag.IsSuccess = "Password change successfully";
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public virtual ActionResult Forgetpassword()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Forgetpassword(string email)
        {
            try
            {
                tblUser User = dbEntities.tblUsers.Where(x => x.Email_Adress == email).FirstOrDefault();
                if (User != null)
                {
                    if (User.IsActive == true)
                    {

                        SendForgetPasswordEmail(User.User_ID, User.Email_Adress, User.UserName);
                        ViewBag.IsSuccess = "Password change link is sent to your email address please check your email..!!";
                        return View();
                    }
                    else if (User.IsActive == false)
                    {
                        ViewBag.IsError = "Your Account is deactivated please contact to Admin";
                        return View();
                    }
                }

                else
                {
                    ViewBag.IsError = "Your email is not registered";
                    return View();
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void SendForgetPasswordEmail(int userId, string email, string name)
        {
            try
            {

                Guid activationCode = Guid.NewGuid();

                tblActivationCode tblActivationCode = new tblActivationCode();
                tblActivationCode.Code = activationCode;
                tblActivationCode.UID = userId;
                dbEntities.tblActivationCodes.Add(tblActivationCode);
                dbEntities.SaveChanges();
                tblSetting sa = dbEntities.tblSettings.Find(1);
                using (MailMessage mm = new MailMessage(sa.Email, email))
                {
                    string link = Request.Url.ToString();
                    link = link.Replace("Forgetpassword", "ChangePasswordnew");
                    mm.Subject = "Password reset";
                    string body = "Hello " + name + ",";
                    body += "<br /><br />Please click the following link to reset your password";
                    body += "<br /><a href = '" + link + "?ActivationCode=" + activationCode + "'>Click here to reset your password.</a>";
                    body += "<br /><br />Thanks";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = sa.SMTP;
                    smtp.EnableSsl = Convert.ToBoolean(sa.IsActive);
                    NetworkCredential NetworkCred = new NetworkCredential(sa.Email, sa.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = Convert.ToInt32(sa.Port);
                    smtp.Send(mm);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public virtual ActionResult ChangePasswordnew()
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();
            ViewBag.ActivationCode = activationCode;
            ViewBag.error = "Please enter your New Password..!!";
            return View();
        }
        [HttpPost]
        public virtual ActionResult ChangePasswordnew(string newPassword, string activationCode)
        {
            try
            {
                int UserID = 0;
                string constr = ConfigurationManager.ConnectionStrings["CarsEntities"].ConnectionString;
                if (activationCode != "00000000-0000-0000-0000-000000000000")
                {
                    List<tblActivationCode> info = dbEntities.tblActivationCodes.SqlQuery("Select * from tblActivationCode where Code = '" + activationCode + "'").ToList();
                    foreach (var x in info)
                    {
                        UserID = Convert.ToInt32(x.UID);
                    }
                    if (UserID > 0)
                    {
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM tblActivationCode WHERE Code = @Code; update [tblUser] set Password='" + newPassword + "' where User_ID=" + UserID + ";"))
                            {
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.AddWithValue("@Code", activationCode);
                                    cmd.Connection = con;
                                    con.Open();
                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    con.Close();
                                    if (rowsAffected == 2)
                                    {
                                        return RedirectToAction("Login", "Account");
                                    }
                                    else
                                    {
                                        return View();
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        ViewBag.error = "Password already changed please make new request..!!";
                        return View();
                    }
                }
                return RedirectToAction("Index", "Account");
            }
            catch (Exception)
            {

                throw;
            }

        }
        public virtual ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Register([Bind(Include = "UserID, Password,Email_Adress,UserName")] tblUser user)
        {
            try
            {
                tblUser User = dbEntities.tblUsers.Where(x => x.Email_Adress == user.Email_Adress).FirstOrDefault();
                if (User == null)
                {
                    user.ImagePath = "\\UserImages\\user.jpg";
                    user.Create_Date = DateTime.Now;
                    user.IsActive = true;
                    user.Access_Level_ID = 2;
                    user.UserType = "User";
                    user.MobileNo = "";
                    dbEntities.tblUsers.Add(user);
                    int rows = dbEntities.SaveChanges();
                    if (rows > 0)
                    { return RedirectToAction("Login", new { Email = user.Email_Adress, password = user.Password }); }
                }
                else
                {
                    ViewBag.error = "Supplied email already registered";
                    return View();
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}