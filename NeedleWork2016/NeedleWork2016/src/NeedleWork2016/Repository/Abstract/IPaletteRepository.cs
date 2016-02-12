using NeedleWork2016.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.Repository.Abstract
{
    public interface IPaletteRepository
    {
        IEnumerable<Palette> GetPalettes(string userId);
        IEnumerable<Palette> GetUserPalettes(string userId);
        Palette AddUserPalette(Palette palette);
        bool UpdatePalette(Palette palette);
        bool DeletePalette(int paletteId);
    }
}
