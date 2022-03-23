using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cars411.Models
{
    public class ModelHelper
    {
        public List<SelectListItem> ToSelectItemList(dynamic values,int type)
        {
            List<SelectListItem> tempList = null;
            if (type == 1)
            {

                if (values != null)
                {
                    tempList = new List<SelectListItem>();
                    foreach (var v in values)
                    {
                        tempList.Add(new SelectListItem { Text = v.Maketitle, Value = Convert.ToString(v.MakeID) });
                    }
                    tempList.TrimExcess();
                }
            }
            if (type == 2)
            {

                if (values != null)
                {
                    tempList = new List<SelectListItem>();
                    foreach (var v in values)
                    {
                        tempList.Add(new SelectListItem { Text = v.Modeltitle, Value = Convert.ToString(v.ModelID) });
                    }
                    tempList.TrimExcess();
                }
            }
            if (type == 3)
            {

                if (values != null)
                {
                    tempList = new List<SelectListItem>();
                    foreach (var v in values)
                    {
                        tempList.Add(new SelectListItem { Text = v.Yeartitle, Value = Convert.ToString(v.YearID) });
                    }
                    tempList.TrimExcess();
                }
            }
            return tempList;
        }
    }
}