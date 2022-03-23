using Cars411.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cars411.Controllers
{
    [FilterConfig.NoDirectAccess]
    [FilterConfig.AuthorizeActionFilter]
    public partial class MakeModelController : Controller
    {
        private Cars411Entities dbEntities = new Cars411Entities();
        public virtual ActionResult Index()
        {
            try
            {
                ViewBag.Make = dbEntities.tblMakes.ToList();
                ViewBag.Model = dbEntities.tblModels.ToList();
                ViewBag.Year = dbEntities.tblYears.ToList();
                if (TempData["TabStatus"] == null)
                {
                    TempData["TabStatus"] = 1;
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public virtual ActionResult SaveData(int? ID, string Name, int type, string action)
        {
            try
            {
                if (action == "save")
                {
                    if (type == 1)
                    {
                        tblMake make = new tblMake();
                        make.Maketitle = Name;
                        dbEntities.tblMakes.Add(make);
                        dbEntities.SaveChanges();
                        TempData["TabStatus"] = 1;
                    }
                    if (type == 2)
                    {
                        tblModel model = new tblModel();
                        model.Modeltitle = Name;
                        dbEntities.tblModels.Add(model);
                        dbEntities.SaveChanges();
                        TempData["TabStatus"] = 2;
                    }
                    if (type == 3)
                    {
                        tblYear year = new tblYear();
                        year.Yeartitle = Name;
                        dbEntities.tblYears.Add(year);
                        dbEntities.SaveChanges();
                        TempData["TabStatus"] = 3;
                    }

                    return RedirectToAction("Index");
                }
                else if (action == "Edit")
                {
                    if (type == 1)
                    {
                        tblMake make = dbEntities.tblMakes.Find(ID);
                        make.Maketitle = Name;
                        dbEntities.Entry(make).State = EntityState.Modified;
                        dbEntities.SaveChanges();
                        TempData["TabStatus"] = 1;
                    }
                    if (type == 2)
                    {
                        tblModel model = dbEntities.tblModels.Find(ID);
                        model.Modeltitle = Name;
                        dbEntities.Entry(model).State = EntityState.Modified;
                        dbEntities.SaveChanges();
                        TempData["TabStatus"] = 2;
                    }
                    if (type == 3)
                    {
                        tblYear year = dbEntities.tblYears.Find(ID);
                        year.Yeartitle = Name;
                        dbEntities.Entry(year).State = EntityState.Modified;
                        dbEntities.SaveChanges();
                        TempData["TabStatus"] = 3;
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteData(int? id, int type)
        {
            try
            {
                if (type == 1)
                {
                    tblMake make = dbEntities.tblMakes.Find(id);
                    if (make != null)
                    {
                        dbEntities.tblMakes.Remove(make);
                        dbEntities.SaveChanges();
                    }
                    TempData["TabStatus"] = 1;
                }
                if (type == 2)
                {
                    tblModel model = dbEntities.tblModels.Find(id);
                    if (model != null)
                    {
                        dbEntities.tblModels.Remove(model);
                        dbEntities.SaveChanges();
                    }
                    TempData["TabStatus"] = 2;

                }
                if (type == 3)
                {
                    tblYear year = dbEntities.tblYears.Find(id);
                    if (year != null)
                    {
                        dbEntities.tblYears.Remove(year);
                        dbEntities.SaveChanges();
                    }
                    TempData["TabStatus"] = 3;
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

        }




    }
}