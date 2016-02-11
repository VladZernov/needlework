using NeedleWork2016.Entities;
using NeedleWork2016.Models;
using System.Collections.Generic;
using System.Linq;

namespace NeedleWork2016.Repository
{
    //class to work with User Data
    public class UserProfileRepository
    {
        //method with using predicate
        //public static List<ApplicationUser> GetAllUsers(Func<ApplicationUser, bool> predicate)
        //{
            //...
        //}

        //select data about all users
        public static List<ApplicationUser> GetAllUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.ApplicationUser.Select(x=>new ApplicationUser()
                {
                    Id=x.Id,
                    Email=x.Email,
                    FirstName=x.FirstName,
                    LastName=x.LastName
                }).ToList();
            }
        }

        //User editing by Id
        public static void EditUser(ApplicationUser user)
        {
            using (var context = new NeedleWork2016Context())
            {
                var User = context.ApplicationUser.Where(c => c.Id == user.Id).FirstOrDefault();
                if (User != null)
                {
                    User.Id = user.Id;
                    User.Email = user.Email;
                    User.FirstName = user.FirstName;
                    User.LastName = user.LastName;
                    context.SaveChanges();
                }
            }
        }

        //User deletion by Id
        public static void DeleteUserData(string _id)
        {
            //GetAllUsers(x=>x.Email=="@mail.ru");

            using (var context = new NeedleWork2016Context())
            {
                //Linq request for delition user by Id
                var User = context.ApplicationUser.Where(c => c.Id == _id).FirstOrDefault();
                context.ApplicationUser.Remove(User);
                context.SaveChanges();
            }
        }

        //mathod for check user registration
        public static bool UserIsRegistered(string email)
        {
            using (var context = new NeedleWork2016Context())
            {
                var user = context.ApplicationUser.Where(m => m.Email == email).FirstOrDefault();
                if (user != null)
                    return true;
                else
                    return false;                    
            }           
        }

        //check email confirmation
        public static bool IsUserConfirmedEmail(string email)
        {
            using (var context = new NeedleWork2016Context())
            {
                if (UserIsRegistered(email))
                {
                    var user = context.ApplicationUser.Where(m => m.Email == email).FirstOrDefault();
                    if (user.EmailConfirmed)
                        return true;
                    else
                        return false;
                }
                return true;
            }
        }

        //confirm user email
        public static void EmailConfirm(string email)
        {
            using (var context = new NeedleWork2016Context())
            {
                var user = context.ApplicationUser.Where(m => m.Email == email).FirstOrDefault();
                user.EmailConfirmed = true;
                context.SaveChanges();                  
            }

        }
    }
}
