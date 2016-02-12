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
using NeedleWork2016.ViewModels.LocalizationViewModels;
using System.Collections;
using NeedleWork2016.Repository.Abstract;

namespace NeedleWork2016.Controllers
{
    public class PalettesController : Controller
    {
        IRepositoryContainer _repository;

        private NeedleWork2016Context _context;

        public PalettesController(NeedleWork2016Context context, IRepositoryContainer rep)
        {
            _context = context;
            _repository = rep;
        }

        //GET: Palettes
        public IActionResult Index()
        {
            PaletteIndexLocalizationViewModel model = new PaletteIndexLocalizationViewModel();
            model.Title = "Test title";
            model.Layout = ResourceReader.GetLayoutLocalizationViewModel();
            return View(model);
        }

        //Get palettes
        [HttpGet]
        public JsonResult GetPalettes()
        {
            try
            {
                string id = User.GetUserId();
                PaletteListViewModel model = new PaletteListViewModel()
                {
                    Palettes = _repository.PaletteRepository.GetPalettes(User.GetUserId())
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

        //Get palettes for current user
        [HttpGet]
        public JsonResult GetUserPalettes()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    PaletteListViewModel model = new PaletteListViewModel()
                    {
                        Palettes = _repository.PaletteRepository.GetUserPalettes(User.GetUserId())
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

        //Remove user palette by id
        [HttpDelete]
        public JsonResult RemovePalette(int id)
        {
            try
            {
                if (_repository.PaletteRepository.DeletePalette(id))
                {
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
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    palette.IdUser = User.GetUserId();
                    Palette AddedPalette = _repository.PaletteRepository.AddUserPalette(palette);
                    return Json(new PaletteViewModel(AddedPalette) { Result = new ManipulationResult(Result.Success) });
                }
                else
                    return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, "User is unauthenticated") });
            }
            catch (Exception ex)
            {
                string ErrorMsg = ErrorHandler.HandleException(ex);
                if (ErrorMsg != "")
                    return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, ErrorMsg) });
                else
                    return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Exeption, ex) });
            }
        }

        //Edit palette using inputed data
        [HttpPost]
        public JsonResult EditPalette(Palette palette) //now its gets int id, string name
        {
            try
            {
                palette.IdUser = User.GetUserId();
                if (_repository.PaletteRepository.UpdatePalette(palette))
                {
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

        //-----------------
        //ColorsMethods
        //-----------------

        //Get colors in RGB
        [HttpGet]
        public JsonResult GetRGBColors(int idpalette)
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

        //Create new color for palette
        [HttpPost]
        public JsonResult CreateColor(Color color)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (_context.Palette.Any(p => p.Id == color.IdPalette))
                    {
                        _context.Color.Add(color);
                        _context.SaveChanges();
                        Color AddedColor = _context.Color.OrderByDescending(c => c.Id).FirstOrDefault();
                        ColorViewModel AddedColorView = new ColorViewModel(AddedColor)
                        {
                            Result = new ManipulationResult(Result.Success)
                        };
                        return Json(AddedColorView);
                    }
                    else
                        return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, "Palette doesn't exist") });
                }
                else
                    return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, "User is unauthenticated") });
            }
            catch (Exception ex)
            {
                string ErrorMsg = ErrorHandler.HandleException(ex);
                if (ErrorMsg != "")
                    return Json(new PaletteViewModel() { Result = new ManipulationResult(Result.Error, ErrorMsg) });
                else
                    return Json(new ManipulationResult(Result.Exeption, ex));
            }
        }

        //Edit color using inputed data
        [HttpPost]
        public JsonResult EditColor(Color color) 
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

    }
}
