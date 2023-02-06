using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TTB.CAVU.ParkingSpaces.DataAccess.Config;
using TTB.CAVU.ParkingSpaces.DataAccess.Model;

namespace TTB.CAVU.ParkingSpaces.DataAccess.Repository
{
    public class GenericRepositoryService<TDoc> : IGenericRepositoryService<TDoc> where TDoc : AppDataModel
    {
        private readonly IMongoCollection<TDoc> _collection;

        public GenericRepositoryService(IMongoCollection<TDoc> collection)
        {
            _collection = collection;
        }

        public IEnumerable<TDoc> Query()
        {
            return _collection.AsQueryable();
        }

        public async Task<TDoc> CreateAsync(TDoc doc)
        {
            await _collection.InsertOneAsync(doc);
            return doc;
        }

        public Task DeleteAsync(TDoc doc)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TDoc>> GetAsync(Expression<Func<TDoc, bool>>? expression = null)
        {
            if (expression is null)
            {
                expression = x => true;
            }
            var response = await _collection.FindAsync<TDoc>(expression);
            return await response.ToListAsync();
        }

        public async Task<TDoc> GetFirstAsync(Expression<Func<TDoc, bool>>? expression = null)
        {
            if (expression is null)
            {
                expression = x => true;
            }
            var response = await _collection.FindAsync<TDoc>(expression);
            return await response.SingleOrDefaultAsync();
        }

        public async Task<TDoc> ModifyAsync(Expression<Func<TDoc, bool>> expression, TDoc doc)
        {
            await _collection.UpdateOneAsync<TDoc>(expression, new ObjectUpdateDefinition<TDoc>(doc));
            return doc;
        }
    }
}
