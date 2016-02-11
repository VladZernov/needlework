using NeedleWork2016.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.ViewModels.Palettes
{
    public class ColorListViewModel
    {
        public IEnumerable<ColorViewModel> Colors { get; set; }
        public ManipulationResult Result { get; set; }
    }
}
