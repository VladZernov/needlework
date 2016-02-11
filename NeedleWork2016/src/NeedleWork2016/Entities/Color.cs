using System;
using System.Collections.Generic;

namespace NeedleWork2016.Entities
{
    public partial class Color
    {
        public int Id { get; set; }
        public string Hex { get; set; }
        public int IdPalette { get; set; }
        public string Name { get; set; }

        public virtual Palette IdPaletteNavigation { get; set; }
    }
}
