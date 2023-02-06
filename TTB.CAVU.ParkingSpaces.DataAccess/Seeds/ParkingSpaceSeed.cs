using Microsoft.Extensions.Logging;
using TTB.CAVU.ParkingSpaces.DataAccess.Model;
using TTB.CAVU.ParkingSpaces.DataAccess.Repository;

namespace TTB.CAVU.ParkingSpaces.DataAccess.Seeds
{
    public class ParkingSpaceSeed
    {
        public static async Task Seed(IAppRepository repo, ILogger<DatabaseSeedManager> logger)
        {
            for (var i = 0; i < 10; i++)
            {
                await repo.GetRepository<ParkingSpace>().CreateAsync(new ParkingSpace());
            }
        }
    }
}
