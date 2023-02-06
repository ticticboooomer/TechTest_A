using TTB.CAVU.ParkingSpaces.DataAccess.Model;

namespace TTB.CAVU.ParkingSpaces.DataAccess.Repository
{
    public interface IAppRepository
    {
        IGenericRepositoryService<T> GetRepository<T>() where T : AppDataModel;
    }
}
