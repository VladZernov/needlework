using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeedleWork2016.Core;
using NeedleWork2016.Models;

namespace NeedleWork2016.ViewModels.Palettes
{
    public class RGBColorsViewModel
    {
        public ArrayList RGBColors { get; private set; }

        public ManipulationResult Result { get; set; }

        public RGBColorsViewModel(IQueryable<Color> colors)
        {
            RGBColors = new ArrayList();
            foreach (Color color in colors)
            {
                object[] rgb = ColorConverter.HexToRGB(color.Hex, color.Name);
                RGBColors.Add(rgb);
            };
        }
    }
}
