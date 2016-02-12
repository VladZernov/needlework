using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.ViewModels.Home
{
    public class HomeViewModel
    {
        public string Title { get; set; }
        public string FirstBlockName { get; set; }
        public string[] FirstBlockText { get; set; }
        public string SecondBlockName { get; set; }
        public string[] SecondBlockText { get; set; }
        public string ThirdBlockName { get; set; }
        public string[] ThirdBlockText { get; set; }
        public LocalizationViewModels.LayoutLocalizationViewModel Layout { get; set; }
    }
}
