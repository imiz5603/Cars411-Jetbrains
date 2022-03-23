using Cars411.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Cars411.Controllers
{
    public partial class CarsController : Controller
    {
        private Cars411Entities dbEntities = new Cars411Entities();
        public virtual ActionResult Index()
        {
            try
            {
                ViewBag.Comfort = "";
                ViewBag.Safety = "";
                ViewBag.Reliability = "";
                ViewBag.year = "";
                List<getBestCarRating_Result2> getCarRating_Result = dbEntities.getBestCarRating(1, 0, 0, 0, " group by  P.PostID,P.PostDescription,p.PostImage,P.PostedDate,P.PostTitle" +
                    " Having IsNull(sum(C.Comfort)/count(C.Comfort),0)>0  and  IsNull(sum(C.Safety)/count(C.Safety),0)>0 and IsNull(sum(C.Relaiability)/count(C.Relaiability),0)>0").ToList();
                ViewBag.CarList = getCarRating_Result;
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public virtual ActionResult Index(int Comfort = 0, int Safety = 0, int Reliability = 0, int year = 0)
        {
            try
            {
                if (Comfort == 0 && Safety == 0 && Reliability == 0 && year == 0)
                {
                    List<getBestCarRating_Result2> getCarRating_Result = dbEntities.getBestCarRating(1, 0, 0, 0, " group by  P.PostID,P.PostDescription,p.PostImage,P.PostedDate,P.PostTitle" +
                                       " Having IsNull(sum(C.Comfort)/count(C.Comfort),0)>0  and  IsNull(sum(C.Safety)/count(C.Safety),0)>0 and IsNull(sum(C.Relaiability)/count(C.Relaiability),0)>0").ToList();
                    ViewBag.CarList = getCarRating_Result;
                    return View();
                }
                else
                {
                    string cwhere = "";
                    if (year > 0)
                    {
                        cwhere = cwhere + "join tblYear Y on Y.YearID = P.YearID where Y.Yeartitle =" + year;
                        cwhere = cwhere + " group by  P.PostID,P.PostDescription,p.PostImage,P.PostedDate,P.PostTitle ";
                    }
                    if (year == 0)
                    {
                        cwhere = cwhere + " group by  P.PostID,P.PostDescription,p.PostImage,P.PostedDate,P.PostTitle ";

                    }
                    if (Comfort > 0 || Safety > 0 || Reliability > 0)
                    {
                        cwhere = cwhere + " Having ";
                    }
                    if (Comfort > 0)
                    {
                        cwhere = cwhere + " IsNull(sum(C.Comfort)/count(C.Comfort),0)>=" + Comfort;
                    }
                    if (Safety > 0 && Comfort > 0)
                    {
                        cwhere = cwhere + " and  IsNull(sum(C.Safety)/count(C.Safety),0)>=" + Safety;
                    }
                    if (Safety > 0 && Comfort <= 0)
                    {
                        cwhere = cwhere + " IsNull(sum(C.Safety)/count(C.Safety),0)>=" + Safety;
                    }
                    if (Reliability > 0 && (Safety > 0 || Comfort > 0))
                    {
                        cwhere = cwhere + " and IsNull(sum(C.Relaiability)/count(C.Relaiability),0)>=" + Reliability;
                    }
                    if (Reliability > 0 && (Safety <= 0 && Comfort <= 0))
                    {
                        cwhere = cwhere + " IsNull(sum(C.Relaiability)/count(C.Relaiability),0)>=" + Reliability;
                    }
                    List<getBestCarRating_Result2> getCarRating_Result = dbEntities.getBestCarRating(Safety, Comfort, Reliability, year, cwhere).ToList();
                    ViewBag.CarList = getCarRating_Result;

                    ViewBag.Comfort = "";
                    ViewBag.Safety = "";
                    ViewBag.Reliability = "";
                    ViewBag.year = "";
                    if (Comfort > 0)
                    {
                        ViewBag.Comfort = Comfort;
                    }
                    if (Safety > 0)
                    {
                        ViewBag.Safety = Safety;
                    }
                    if (Reliability > 0)
                    {
                        ViewBag.Reliability = Reliability;
                    }
                    if (year > 0)
                    {
                        ViewBag.year = year;
                    }
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public virtual ActionResult Details(int? id)
        {
            try
            {
                List<Double> dataPoints1 = new List<Double>();

                getCarRating_Result2 getCarRating_Result = dbEntities.getCarRating(id, 0, 0, 0).FirstOrDefault();

                if (getCarRating_Result == null)
                {
                    dataPoints1.Add((0));
                    dataPoints1.Add((0));
                    dataPoints1.Add((0));
                    ViewBag.Average = 0;
                }
                else
                {
                    dataPoints1.Add(Convert.ToDouble(getCarRating_Result.Comfort));
                    dataPoints1.Add(Convert.ToDouble(getCarRating_Result.Safety));
                    dataPoints1.Add(Convert.ToDouble(getCarRating_Result.Relaiability));
                    ViewBag.Average = getCarRating_Result.Aveg;
                }
                ViewBag.Comment = dbEntities.tblComments.Where(x => x.PostID == id).ToList();
                ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
                tblPost post = dbEntities.tblPosts.Where(x => x.PostID == id).FirstOrDefault();
                return View(post);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public virtual ActionResult Details([Bind(Include = "CommentID,Comment,Relaiability,Safety,Comfort,PostID,UserName,UserImage,ParentID")] tblComment comment1)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                List<Double> dataPoints1 = new List<Double>();

                if (comment1.Relaiability == null)
                {
                    comment1.Relaiability = 0;
                }
                if (comment1.Safety == null)
                {
                    comment1.Safety = 0;
                }
                if (comment1.Comfort == null)
                {
                    comment1.Comfort = 0;
                }
                if (comment1.ParentID > 0)
                {
                    comment1.IsParent = 0;
                    comment1.Comfort = 5;
                    comment1.Safety = 5;
                    comment1.Relaiability = 5;
                }
                else
                {
                    comment1.IsParent = 1;
                }
                comment1.UserID = Convert.ToInt32(Session["UserID"]);
                comment1.UserName = Convert.ToString(Session["UserName"]);
                comment1.UserImage = Convert.ToString(Session["ImagePath"]);
                comment1.CommentDate = DateTime.Now;
                comment1.UserType = Convert.ToString(Session["UserType"]);
                dbEntities.tblComments.Add(comment1);
                dbEntities.SaveChanges();
                tblPost post = dbEntities.tblPosts.Where(x => x.PostID == comment1.PostID).FirstOrDefault();
                ViewBag.Comment = dbEntities.tblComments.Where(x => x.PostID == comment1.PostID).ToList();
                getCarRating_Result2 getCarRating_Result = dbEntities.getCarRating(comment1.PostID, 0, 0, 0).FirstOrDefault();
                if (getCarRating_Result == null)
                {
                    dataPoints1.Add((0));
                    dataPoints1.Add((0));
                    dataPoints1.Add((0));
                    ViewBag.Average = 0;
                }
                else
                {
                    dataPoints1.Add(Convert.ToDouble(getCarRating_Result.Comfort));
                    dataPoints1.Add(Convert.ToDouble(getCarRating_Result.Safety));
                    dataPoints1.Add(Convert.ToDouble(getCarRating_Result.Relaiability));
                    ViewBag.Average = getCarRating_Result.Aveg;
                }
                ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
                return View(post);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public virtual ActionResult AllCarList()
        {
            try
            {
                List<getCarRating_Result2> getCarRating_Result = dbEntities.getCarRating(0, 0, 0, 0).ToList();
                ViewBag.CarList = getCarRating_Result;
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual ActionResult ExpiryServiceDate()
        {
            try
            {
                DateTime date = DateTime.Now.Date;
                tblSetting sa = dbEntities.tblSettings.Find(1);
                List<tblUser> UserList = dbEntities.tblUsers.Where(x => x.LicenseExpiry <= date && x.Access_Level_ID == 2 && x.LicenseExpiry != null).ToList();
                if (UserList.Count > 0)
                {
                    foreach (var user in UserList)
                    {
                        using (MailMessage mm = new MailMessage(sa.Email, user.Email_Adress))
                        {

                            mm.Subject = "License Expiry Date Renew Request";
                            string body = "Hello " + user.UserName + ",";
                            body += "<br /><br />Your license expire date is reached. It is a pretty straightforward message that prompts you to renew your license expiry date. ";
                            body += "<br /><br />Thank You";
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
                }
                List<tblUser> UserList1 = dbEntities.tblUsers.Where(x => x.ServiceExpiry <= date && x.Access_Level_ID == 2 && x.ServiceExpiry != null).ToList();
                if (UserList1.Count > 0)
                {
                    foreach (var user in UserList1)
                    {

                        using (MailMessage mm = new MailMessage(sa.Email, user.Email_Adress))
                        {

                            mm.Subject = "Service Expiry Date Renew Request";
                            string body = "Hello " + user.UserName + ",";
                            body += "<br /><br />Your service expire date is reached. It is a pretty straightforward message that prompts you to renew your service expiry date. ";
                            body += "<br /><br />Thank You";
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