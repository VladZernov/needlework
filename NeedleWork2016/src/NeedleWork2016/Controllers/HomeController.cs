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
using NeedleWork2016.ViewModels.Palettes;

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
        {
            try
            {
                PaletteListViewModel model = new PaletteListViewModel()
                {
                    Palettes = (_context.Palette
                                .Where(p => (User == null) ? (p.IdUser == null) : ((p.IdUser == null) || (p.IdUser == User.GetUserId()))) as IEnumerable<Palette>)
                                .Select(p => new PaletteViewModel(p)),

                    Result = new ManipulationResult(Result.Success)
                };
                return Json(model);
            }
            catch (Exception ex)
            {
                PaletteListViewModel model = new PaletteListViewModel()
                {
                    Result = new ManipulationResult(Result.Exeption, ex)
                };
                return Json(model);
            }
        }

        public JsonResult GetColors(int idpalette)
        {
            try
            {
                var colors = _context.Color.Where(c => c.IdPalette == idpalette);

                RGBColorsViewModel model = new RGBColorsViewModel(colors)
                {
                    Result = new ManipulationResult(Result.Success)
                };
                return Json(model);
            }
            catch (Exception ex)
            {
                PaletteListViewModel model = new PaletteListViewModel()
                {
                    Result = new ManipulationResult(Result.Exeption, ex)
                };
                return Json(model);
            }
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
