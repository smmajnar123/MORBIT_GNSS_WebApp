using Microsoft.EntityFrameworkCore;
using MORBIT_GNSS_APP.DataAccessLayer;
using MORBIT_GNSS_APP.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORBIT_GNSS_APP.Repository.Repository
{
    public class BaseRepository<T>(MorbitGnssAppDbContext morbitGnssAppDbContext) : IBaseRepository<T> where T : class
    {
        private readonly MorbitGnssAppDbContext morbitGnssAppDbContext = morbitGnssAppDbContext;

        public Task AddAsync(T entity)
        {
            morbitGnssAppDbContext.Set<T>().AddAsync(entity);
            return this.morbitGnssAppDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this.morbitGnssAppDbContext.Set<T>().ToListAsync();
        }
    }
}
