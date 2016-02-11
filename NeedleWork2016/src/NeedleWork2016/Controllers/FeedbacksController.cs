using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using NeedleWork2016.Entities;

namespace NeedleWork2016.Controllers
{
    public class FeedbacksController : Controller
    {
        private NeedleWork2016Context _context;

        public FeedbacksController(NeedleWork2016Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Method for create a new feedback
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public bool CreateFeedBack(string text)
        {
            if (User.Identity.IsAuthenticated)
            {
                Feedback _feedback = new Feedback();
                _feedback.Text = text;
                _context.Feedback.Add(_feedback);
                _context.SaveChanges();
               
            }
            return true;
        }
     
    }
}
