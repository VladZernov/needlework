using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using NeedleWork2016.Entities;
using System.Security.Claims;
using NeedleWork2016.ViewModels.LocalizationViewModels;
using NeedleWork2016.Core;

namespace NeedleWork2016.Controllers
{
    public class FeedbacksController : Controller
    {
        private NeedleWork2016Context _context;
        private readonly Core.Error.ListOfErrors _listOfErrors;

        public FeedbacksController(NeedleWork2016Context context)
        {
            _context = context;    
        }

        // GET: Feedbacks1
        public IActionResult Index()
        {
           /* FeedbackIndexLocalizationViewModel model = new FeedbackIndexLocalizationViewModel();
            model.Title = "Test title";
            model.Layout = ResourceReader.GetLayoutLocalizationViewModel();*/
            var needleWork2016Context = _context.Feedback.Include(f => f.IdUserNavigation);
           
            return View(needleWork2016Context.ToList());
        }

        // GET: Feedbacks1/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Feedback feedback = _context.Feedback.Single(m => m.Id == id);
            if (feedback == null)
            {
                return HttpNotFound();
            }

            return View(feedback);
        }

        /// <summary>
        /// Method for create a new feedback
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateFeedBack(string text)
        {
            if (User.Identity.IsAuthenticated)
            {
                Feedback _feedback = new Feedback();
                _feedback.Text = text;
                _feedback.IdUser = User.GetUserId();
                _context.Feedback.Add(_feedback);
                _context.SaveChanges();

            }

            return _listOfErrors[2008].ToJson();
        }


        // GET: Feedbacks1/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Feedback feedback = _context.Feedback.Single(m => m.Id == id);
            if (feedback == null)
            {
                return HttpNotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = _context.Feedback.Single(m => m.Id == id);
            _context.Feedback.Remove(feedback);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
