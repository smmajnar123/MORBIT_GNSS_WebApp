using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MORBIT_GNSS_APP.DataAccessLayer.Models;
using MORBIT_GNSS_APP.Repository.IRepository;
using Microsoft.Extensions.Options;

namespace MORBIT_GNSS_APP.Repository.Repository
{
    public class GnssDataRepository : IGnssDataRepository
    {
        //private readonly MorbitGnssAppDbContext _dbContext;
        //public GnssDataRepository(MorbitGnssAppDbContext morbitGnssAppDbContext ) 
        //{ 
        //     _dbContext = morbitGnssAppDbContext;
        //}

        private readonly IMongoCollection<GnssData> _collection;
        public GnssDataRepository(IMongoClient client, IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<GnssData>(settings.CollectionName);
        }

        public async Task<int> AddAsync(GnssData entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
                return 1; // Success
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error inserting GnssData: {ex.Message}");
                return 0; // Failure
            }
        }
        public async Task<IEnumerable<GnssData>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
