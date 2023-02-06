using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTB.CAVU.ParkingSpaces.DataAccess.Model
{
    public class SpaceBooking : AppDataModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ObjectId SpaceId { get; set; }
    }
}
