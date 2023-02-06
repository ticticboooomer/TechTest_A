using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTB.CAVU.ParkingSpaces.DataAccess.Model;
using TTB.CAVU.ParkingSpaces.DataAccess.Repository;

namespace TTB.CAVU.ParkingSpaces.DataAccess.Seeds
{
    public class DatabaseSeedManager : IDatabaseSeedManager
    {
        private readonly IAppRepository _repo;
        private readonly ILogger<DatabaseSeedManager> _logger;

        public DatabaseSeedManager(IAppRepository repo, ILogger<DatabaseSeedManager> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task RunSeeds()
        {
            // To save time, i did the seeding manually with a static function and query, However, I could have made a similar solution to the AppRepository but with a list of classes which would seed the db. 
            // There is very little needed to be seeded, didnt want to waste time with over complicating the below. 
            // In a production scenario, I would likely use a very different solution (mentioned above).
            var seedQuery = await _repo.GetRepository<Seed>().GetAsync(x => x.Index == 0 && !x.IsComplete);
            if (!seedQuery.Any())
            {
                await ParkingSpaceSeed.Seed(_repo, _logger);
                await _repo.GetRepository<Seed>().CreateAsync(new Seed
                {
                    Index = 0,
                });
            }
        }
    }
}
