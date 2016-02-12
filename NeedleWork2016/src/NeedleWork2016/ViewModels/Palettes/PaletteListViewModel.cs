using System;
using System.Collections.Generic;
using System.Linq;
using NeedleWork2016.Core;

namespace NeedleWork2016.ViewModels.Palettes
{
    public class PaletteListViewModel
    {
        public IEnumerable<PaletteViewModel> Palettes { get; set; }
        public ManipulationResult Result { get; set; }

    }
}
