using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTB.CAVU.ParkingSpaces.DataAccess.Seeds
{
    public interface IDatabaseSeedManager
    {
        Task RunSeeds();
    }
}
