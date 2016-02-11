using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using NeedleWork2016.Entities;
using System.Web.Security;
using System.Security.Claims;
using System.Collections;
using NeedleWork2016.Core;
using System.Web;
using NeedleWork2016.ViewModels.Home;

namespace NeedleWork2016.Controllers
{
    public class HomeController : Controller
    {
        private NeedleWork2016Context _context;

        public HomeController(NeedleWork2016Context context)
        {
            _context = context;
        }

        public JsonResult GetPalettes()
        {                                                       //user confirmation
            var PaletteList = _context.Palette.Where(p => (p.IdUser == User.GetUserId()) || (p.IdUser == null)).Select( 
                    a => new
                    {
                        a.Id,
                        a.Name,
                    }); //User == null ? 
            return Json(PaletteList);
        }

        public JsonResult GetColors(int idpalette)
        {
            var colors = _context.Color.Where(c => c.IdPalette == idpalette);
            ArrayList rgbcolors = new ArrayList();
            foreach (Color color in colors)
            {
                object[] rgb = ColorConverter.HexToRGBwithName(color.Hex,color.Name);
                rgbcolors.Add(rgb);
            }; //make fat model
            return Json(rgbcolors);
        }

        public IActionResult Index()
        {
            HomeViewModel model = (HomeViewModel)ResourceReader.ParseJson<HomeViewModel>("../Resources/Home/Index");
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
