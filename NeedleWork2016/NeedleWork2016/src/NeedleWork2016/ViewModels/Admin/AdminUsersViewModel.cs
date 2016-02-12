using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeedleWork2016.ViewModels.LocalizationViewModels;

namespace NeedleWork2016.ViewModels.Admin
{
    public class AdminUsersViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LstName { get; set; }
        public string Email { get; set; }
        public LayoutLocalizationViewModel Layout { get; set; }
    }
}
