﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cars411
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class AuthorizeActionFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                Controller controller = filterContext.Controller as Controller;

                if (controller != null)
                {
                    if (session["UserID"] == null || session["UserName"] == null || session["ImagePath"] == null || session["Menu"] == null || session["UserType"] == null || session["Email"] == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                                             RouteValueDictionary(new { controller = "Account", action = "Login" }));
                    }
                }

                base.OnActionExecuting(filterContext);
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.HttpContext.Request.UrlReferrer == null ||
         filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                                              RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
            }
        }
    }
}
