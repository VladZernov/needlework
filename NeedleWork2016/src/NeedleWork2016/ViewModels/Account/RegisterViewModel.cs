using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.ViewModels.Account
{
    public class RegisterViewModel
    {
        //Type of the field - String
        [DataType(DataType.Text)]
        //This field is required
        [Required(ErrorMessage = "You must enter a first name")]
        //Display field on form
        [Display(Name = "First name*")]
        public string FirstName { get; set; }

        //Type of the field - String
        [DataType(DataType.Text)]
        //This field is required
        [Required(ErrorMessage = "You must enter a last name")]
        //Display field on form
        [Display(Name = "Last name*")]
        public string LastName { get; set; }


        [EmailAddress]
        //Display field on form
        [Display(Name = "Email*")]
        //This field is required
        [Required(ErrorMessage = "You must enter E-mail")]
        //Validation of E-mail
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        //Length of string
        [StringLength(100, ErrorMessage = "The password must contain at least {2} characters", MinimumLength = 6)]
        //Type of the field - Password
        [DataType(DataType.Password)]
        //Display field on form
        [Display(Name = "Password*")]
        //This field is required
        [Required(ErrorMessage = "You must enter your password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "Password and confirmation don't match")]
        public string ConfirmPassword { get; set; }
    }
}
