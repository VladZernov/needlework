using System;
using System.Collections.Generic;

namespace NeedleWork2016.Entities
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string IdUser { get; set; }
        public virtual AspNetUsers IdUserNavigation { get; set; }
    }
}
