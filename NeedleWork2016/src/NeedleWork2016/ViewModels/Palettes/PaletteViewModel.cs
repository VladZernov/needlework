using NeedleWork2016.Core;
using NeedleWork2016.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.ViewModels.Palettes
{
    public class PaletteViewModel
    {
        private int _id;
        private string _name;

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        
        //make hidden for nullable
        public ManipulationResult Result { get; set; }

        public PaletteViewModel(Palette model)
        {
            _id = model.Id;
            _name = model.Name;
        }

        public PaletteViewModel()
        {
        }
    }
}
