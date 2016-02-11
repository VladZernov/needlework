using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeedleWork2016.Entities;
using NeedleWork2016.Core;
using Newtonsoft.Json;

namespace NeedleWork2016.ViewModels.Palettes
{
    public class ColorViewModel
    {
        private int _id;
        private string _name;
        private string _hex;
        private int _idPalette;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ManipulationResult Result { get; set; }

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

        public string Hex
        {
            get
            {
                return _hex;
            }

            set
            {
                _hex = value;
            }
        }

        public int IdPalette
        {
            get
            {
                return _idPalette;
            }

            set
            {
                _idPalette = value;
            }
        }

        public ColorViewModel(Color model)
        {
            _id = model.Id;
            _name = model.Name;
            _hex = model.Hex;
            _idPalette = model.IdPalette;
        }

        // add property for rgb generation
    }
}
