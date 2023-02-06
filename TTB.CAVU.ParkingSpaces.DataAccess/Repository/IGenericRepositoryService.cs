using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TTB.CAVU.ParkingSpaces.DataAccess.Model;

namespace TTB.CAVU.ParkingSpaces.DataAccess.Repository
{
    public interface IGenericRepositoryService<TDoc> where TDoc : AppDataModel
    {
        Task<IEnumerable<TDoc>> GetAsync(Expression<Func<TDoc, bool>>? expression = null);
        Task<TDoc> GetFirstAsync(Expression<Func<TDoc, bool>>? expression = null);
        Task<TDoc> ModifyAsync(Expression<Func<TDoc, bool>> expression, TDoc doc);
        Task DeleteAsync(TDoc doc);
        Task<TDoc> CreateAsync(TDoc doc);
        IEnumerable<TDoc> Query();
    }
}
