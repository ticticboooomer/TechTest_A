using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTB.CAVU.ParkingSpaces.Services.Model.Parking;

namespace TTB.CAVU.ParkingSpaces.Services.Parking
{
    public interface IParkingService
    {
        Task<List<AvailableSpacesModel>> GetAvailableSpacesAsync(DateTime queryStart, DateTime queryEnd);
    }
}
