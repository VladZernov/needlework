using NeedleWork2016.Models;
using System;
using System.Collections.Generic;

namespace NeedleWork2016.Models
{
    public partial class Palette
    {
        public Palette()
        {
            ColorList = new HashSet<Color>();
        }

        public int Id { get; set; }
        public string IdUser { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Color> ColorList { get; set; }
        public virtual ApplicationUser IdUserNavigation { get; set; }
    }
}
