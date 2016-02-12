using NeedleWork2016.Entities;
using NeedleWork2016.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.Repository.Concrete
{
    public class RepositoryContainer : IRepositoryContainer
    {
        private NeedleWork2016Context _context;

        public RepositoryContainer(NeedleWork2016Context context)
        {
            _context = context;
        }

        public IPaletteRepository PaletteRepository
        {
            get { return new PaletteRepository(_context); }
        }
    }
}
