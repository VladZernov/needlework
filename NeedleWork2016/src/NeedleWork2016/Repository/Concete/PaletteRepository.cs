using NeedleWork2016.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using NeedleWork2016.Repository.Abstract;
using Microsoft.AspNet.Mvc.Rendering;


namespace NeedleWork2016.Repository.Concrete
{
    public class PaletteRepository : IPaletteRepository
    {
        private readonly NeedleWork2016Context _context;

        public PaletteRepository(NeedleWork2016Context context)
        {
            _context = context;
        }

        public IEnumerable<Palette> GetUserPalettes(string userId)
        {
            return _context.Palette.Where(p => p.IdUser == userId);
        }

        public IEnumerable<Palette> GetPalettes(string userId)
        {
            return _context.Palette.Where(p => (p.IdUser == null) || (p.IdUser == userId));
        }

        public Palette AddUserPalette(Palette palette)
        {
            _context.Palette.Add(palette);
            _context.SaveChanges();
            Palette AddedPalette = _context.Palette.OrderByDescending(p => p.Id).FirstOrDefault();
            return AddedPalette;
        }

        public bool UpdatePalette(Palette palette)
        {
            if (_context.Palette.Any(p => p.Id == palette.Id))
            {
                _context.Update(palette);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeletePalette(int paletteId)
        {
            Palette palette = _context.Palette.FirstOrDefault(p => p.Id == paletteId);
            if (palette != null)
            {
                _context.Palette.Remove(palette);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}

