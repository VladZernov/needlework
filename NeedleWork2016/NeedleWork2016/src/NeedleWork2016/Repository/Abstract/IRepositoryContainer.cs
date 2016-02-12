using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.Repository.Abstract
{
    public interface IRepositoryContainer
    {
        IPaletteRepository PaletteRepository { get; }

    }
}
