using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.Entities
{
    public class UsersPDF
    {
        public int Id { get; set; }
        public byte[] PdfData { get; set; }
        public string IdUser { get; set; }
        public virtual AspNetUsers IdUserNavigation { get; set; }
    }
}
