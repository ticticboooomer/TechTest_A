using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TTB.CAVU.ParkingSpaces.DataAccess.Model;
using TTB.CAVU.ParkingSpaces.DataAccess.Repository;
using TTB.CAVU.ParkingSpaces.Services.Model.Parking;

namespace TTB.CAVU.ParkingSpaces.Services.Parking
{
    public class ParkingService : IParkingService
    {
        private readonly IAppRepository _repository;

        public ParkingService(IAppRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AvailableSpacesModel>> GetAvailableSpacesAsync(DateTime queryStart, DateTime queryEnd)
        {
            var overlappingBookings = _repository.GetRepository<SpaceBooking>().Query().Where(x => x.StartDate <= queryEnd && queryStart <= x.EndDate);
            var allSpaces = await _repository.GetRepository<ParkingSpace>().GetAsync();
            var date = queryStart;
            var dailyBookings = new List<AvailableSpacesModel>();
            while (date < queryEnd)
            {
                var bookedToday = overlappingBookings.Where(x => x.StartDate <= date && x.EndDate >= date).DistinctBy(x => x.SpaceId).Count();
                dailyBookings.Add(new AvailableSpacesModel
                {
                    AvailableSpaces = allSpaces.Count() - bookedToday,
                    Day = date
                });
                date = date.AddDays(1);
            }
            return dailyBookings;
        }
    }
}
