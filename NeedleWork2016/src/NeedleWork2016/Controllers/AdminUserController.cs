using System.Linq;
using Microsoft.AspNet.Mvc;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using NeedleWork2016.Repository;
using NeedleWork2016.Entities;
using NeedleWork2016.Models;
using Microsoft.AspNet.Identity;

namespace NeedleWork2016.Controllers
{
    public class AdminUserController : Controller
    {
        private ApplicationDbContext _context;

        private readonly Core.Error.ListOfErrors _listOfErrors;

        public AdminUserController(ApplicationDbContext context)
        {
            _context = context;
            _listOfErrors = Core.Error.ErrorStorage.GetListOfErrors();
        }

        public IActionResult Index()
        {
            return View();
        }
        //Serialize to JSON
        [HttpPost]
        public string GetUserData()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<object> list = new List<object>();
            foreach (var i in UserProfileRepository.GetAllUsers().ToList())
            {
                list.Add(new
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName=i.LastName,
                    Email = i.Email,
                    Remove = i.Id
                });
            }
            //serialized list of users and return it
            string temp = serializer.Serialize(list);
            return temp;
        }

        //POST method for call method to user editing by Id from UserProfileRepository
        [HttpPost]
        public void EditUser(ApplicationUser user)
        {
            UserProfileRepository.EditUser(user);
        }

        // GET method to delete User 
        [ActionName("Delete")]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ApplicationUser aspNetUsers = _context.Users.FirstOrDefault(m => m.Id == id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST method for delete user 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            //lambda expressions for delition user by Id
            ApplicationUser aspNetUsers = _context.Users.FirstOrDefault(m => m.Id == id);
            _context.Users.Remove(aspNetUsers);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //POST method for call method from UserProfileRepository
        [HttpGet]
        public void DeleteUserData(string id)
        {
            UserProfileRepository.DeleteUserData(id);
        }
        [HttpPost]
        public void GridSave(string id, string firstname, string lastname, string email)
        {
            if (id!=null&&firstname!=null&&lastname!=null&&email!=null)
            UserProfileRepository.EditUser(new ApplicationUser()
            { Id = id,
              FirstName =firstname,
              LastName =lastname,
              Email =email  });
        }
    }
}
