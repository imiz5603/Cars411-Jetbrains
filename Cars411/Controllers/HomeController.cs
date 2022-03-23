using Cars411.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cars411.Controllers
{
    public partial class HomeController : Controller
    {
        private Cars411Entities dbEntities = new Cars411Entities();
        public virtual ActionResult Index(int? MakeId = 0)
        {
            try
            {
                List<getCarRating_Result2> getCarRating_Result = dbEntities.getCarRating(0, 0, 0, 0).ToList();
                ViewBag.CarList = getCarRating_Result;

                ViewBag.Makes = new ModelHelper().ToSelectItemList(dbEntities.tblMakes, 1).ToList();
                if(MakeId != 0 )
                {
                    var db = dbEntities.tblModels.Select(x=>x.MakeId == MakeId);
                    //ViewBag.Model = new ModelHelper().ToSelectItemList(dbEntities.tblModels.Where(x => x.MakeID == MakeId), 2).ToList();
                    var modelList = new ModelHelper().ToSelectItemList(db, 2).ToList();
                    //ViewBag.Model = new ModelHelper().ToSelectItemList(dbEntities.tblModels, 2).ToList();
                    ViewBag.Model = modelList;
                }
                else
                {

                    ViewBag.Model = new ModelHelper().ToSelectItemList(dbEntities.tblModels, 2).ToList();
                }
                ViewBag.Year = new ModelHelper().ToSelectItemList(dbEntities.tblYears, 3).ToList();
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public virtual ActionResult IndexWithMake(int MakeId = 0)
        {
            try
            {

                return RedirectToAction(MVC.Home.Index(MakeId));
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual ActionResult AllCars()
        {
            List<getCarRating_Result2> getCarRating_Result = dbEntities.getCarRating(0, 0, 0, 0).ToList();
            ViewBag.CarList = getCarRating_Result;
            return View();
        }
        [HttpPost]
        public virtual ActionResult Index(int? ModelID = 0, int? MakeID = 0, int? YearID = 0)
        {
            try
            {
                List<getCarRating_Result2> getCarRating_Result = dbEntities.getCarRating(0, ModelID, MakeID, YearID).ToList();
                ViewBag.CarList = getCarRating_Result;
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

        public virtual ActionResult Blog()
        {
            try
            {
                ViewBag.BlogList = dbEntities.tblBlogs.ToList();
                return View();
            }
            catch (Exception)
            {

                throw;
            }


        }
        public virtual ActionResult BlogPerview(int? id)
        {
            try
            {
                tblBlog blog = dbEntities.tblBlogs.Where(x => x.BlogID == id).FirstOrDefault();
                ViewBag.BlogList = dbEntities.tblBlogs.ToList().Take(5);
                return View(blog);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}