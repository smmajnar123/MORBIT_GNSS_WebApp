using MORBIT_GNSS_APP.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORBIT_GNSS_APP.Repository.IRepository
{
    public interface IGnssDataRepository
    {
        Task<IEnumerable<GnssData>> GetAllAsync();
        Task<int> AddAsync(GnssData entity);
    }
}
