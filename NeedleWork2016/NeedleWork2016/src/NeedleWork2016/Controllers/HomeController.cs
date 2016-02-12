using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using NeedleWork2016.Entities;
using NeedleWork2016.Core;
using System.Web;
using NeedleWork2016.ViewModels.LocalizationViewModels;


namespace NeedleWork2016.Controllers
{
    public class HomeController : Controller
    {
        private NeedleWork2016Context _context;

        public HomeController(NeedleWork2016Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeIndexLocalizationViewModel model = (HomeIndexLocalizationViewModel)ResourceReader.ParseJson<HomeIndexLocalizationViewModel>("../Resources/Home/Index");
            model.Layout = ResourceReader.GetLayoutLocalizationViewModel();
            
            return View(model);
        }

        public string ChangeCulture(string lang)
        {
            string returnUrl = Request.Path;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "de" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie =  new HttpCookie("lang");
            cookie.Value = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Append("lang", cookie.Value);
            //return Redirect(returnUrl);
            return "true";
        }
    }


}
