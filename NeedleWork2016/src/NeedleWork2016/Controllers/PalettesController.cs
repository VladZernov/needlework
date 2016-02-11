using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using NeedleWork2016.Models;
using NeedleWork2016.Entities;
using System.Web.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using NeedleWork2016.Core;
using NeedleWork2016.ViewModels.Palettes;
using System.Collections;

namespace NeedleWork2016.Controllers
{
    public class PalettesController : Controller
    {
        private NeedleWork2016Context _context;

        public PalettesController(NeedleWork2016Context context)
        {
            _context = context;
        }

        //GET: Palettes
        public IActionResult Index()
        {
            return View();
        }

        //Get palettes for current user
        [HttpGet]
        public JsonResult GetPalettes()
        {
            try
            {
                if (User != null)
                {
                    PaletteListViewModel model = new PaletteListViewModel()
                    {
                        Palettes = (_context.Palette
                            .Where(p => p.IdUser == User.GetUserId()) as IEnumerable<Palette>)
                            .Select(p => new PaletteViewModel(p)),

                        Result = new ManipulationResult(Result.Success)
                    };
                    return Json(model);
                }
                else
                {
                    PaletteListViewModel model = new PaletteListViewModel()
                    {
                        Result = new ManipulationResult(Result.Error, "User is unauthenticated")
                    };
                    return Json(model);
                }
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

        //Get colors for particular palette
        [HttpGet]
        public JsonResult GetColors(int idpalette)
        {
            try
            {
                ColorListViewModel model = new ColorListViewModel()
                {
                    Colors = (_context.Color
                        .Where(c => c.IdPalette == idpalette) as IEnumerable<Color>)
                        .Select(c => new ColorViewModel(c)),

                    Result = new ManipulationResult(Result.Success)
                };
                    return Json(model);
            }
            catch (Exception ex)
            {
                return Json(new ManipulationResult(Result.Exeption, ex));
            }
        }

        //Remove color by id
        [HttpDelete]
        public JsonResult RemoveColor(int id)           
        {
            try
            {
                Color color = _context.Color.FirstOrDefault(c => c.Id == id);
                if (color != null)
                {
                    _context.Color.Remove(color);
                    _context.SaveChanges();
                    return Json(new ManipulationResult(Result.Success));
                }
                else
                    return Json(new ManipulationResult(Result.Error, "Inaccessible color"));
            }
            catch (Exception ex)
            {
                return Json(new ManipulationResult(Result.Exeption, ex));
            }
        }

        //Remove user palette by id
        [HttpDelete]
        public JsonResult RemovePalette(int id)
        {
            try
            {
               Palette palette = _context.Palette.FirstOrDefault(p => p.Id == id);
                if (palette != null)
                {
                    _context.Palette.Remove(palette);
                    _context.SaveChanges();
                    return Json(new ManipulationResult(Result.Success));
                }
                else
                    return Json(new ManipulationResult(Result.Error, "Inaccessible palette"));
            }
            catch (Exception ex)
            {
                return Json(new ManipulationResult(Result.Exeption, ex));
            }
        }

        //Create new user palette without colors
        [HttpPost]
        public JsonResult CreatePalette(Palette palette)
        {
            if (User != null)
            {
                try {
                    palette.IdUser = User.GetUserId(); //To transfer this line to View using Razor
                    _context.Palette.Add(palette);
                    _context.SaveChanges();
                    Palette AddedPalette = _context.Palette.OrderByDescending(p => p.Id).FirstOrDefault();
                    return Json( new PaletteViewModel(AddedPalette) { Result = new ManipulationResult(Result.Success) });
                }
                catch (Exception ex)
                {
                    if (ex.HResult == -2146233088)
                        return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, "Palette with the same name is already exist") }); //Make Exeption Handler class
                    else
                        return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Exeption, ex) });
                }
            }
            else
                return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, "User is unauthenticated") });
        }

        //Create new color for palette
        [HttpGet]
        public JsonResult CreateColor(Color color) //int idpalette, string hex, string name; it can be leave as it is but capsulated in object named color
        {
            color = new Color { IdPalette = 3, Name = "345365hg123", Hex = "#FFF16F" };
            if (User != null)                      //catch dublicate in db exeption
            {
                //add idpalette check
                try
                {
                    _context.Color.Add(color);
                    _context.SaveChanges();
                    Color AddedColor = _context.Color.OrderByDescending(c => c.Id).FirstOrDefault(); //
                    ColorViewModel AddedColorView = new ColorViewModel(AddedColor)
                    {
                        Result = new ManipulationResult(Result.Success)
                    };
                    return Json(AddedColorView);
                }
                catch (Exception ex)
                {
                    if (ex.HResult == -2146233088)
                        return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, "Color with the same name is already exist") });
                    else
                    return Json(new ManipulationResult(Result.Exeption, ex));
                }
            }
            else
                return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, "User is unauthenticated") });
        }

        //Edit color using inputed data
        [HttpPost]
        public JsonResult EditColor(Color color) //int id, int name, string hex, int idpalette
        {
            try
            {
                if (_context.Color.Any(c => c.Id == color.Id))
                {
                    _context.Update(color);
                    _context.SaveChanges();
                    return Json(new ManipulationResult(Result.Success));
                }
                else
                    return Json(new ManipulationResult(Result.Error, "Color doesn't exist"));
            }
            catch (Exception ex)
            {
                return Json(new ManipulationResult(Result.Exeption, ex));
            }
        }

        //Edit palette using inputed data
        [HttpPost]
        public JsonResult EditPalette(Palette palette) //now its gets int id, string name
        {
            try
            {
                if (_context.Palette.Contains(palette))
                {
                    palette.IdUser = User.GetUserId();
                    _context.Update(palette);
                    _context.SaveChanges();
                    return Json(new ManipulationResult(Result.Success));
                }
                else
                    return Json(new ManipulationResult(Result.Error, "Palette doesn't exist"));

            }
            catch (Exception ex)
            {
                return Json(new ManipulationResult(Result.Exeption, ex));
            }
        }
}
}
