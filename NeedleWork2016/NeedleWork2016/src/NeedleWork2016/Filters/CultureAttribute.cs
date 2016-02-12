using Microsoft.AspNet.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using Microsoft.AspNet.Mvc;
using NeedleWork2016.Core;


namespace NeedleWork2016.Filters
{
    public class CultureAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Получаем куки из контекста, которые могут содержать установленную культуру
            string cultureName = filterContext.HttpContext.Request.Cookies["lang"];

            /*if (cultureName == null)
                cultureName = "en";*/

            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "de" };
            if (!cultures.Contains(cultureName))
            {
                cultureName = "en";
            }
            ResourceReader.Lang = cultureName;

            return;
        }

    }
}