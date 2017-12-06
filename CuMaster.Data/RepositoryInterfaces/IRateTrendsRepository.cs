using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperFramework.Repository;
using CuMaster.Data.Entities;

namespace CuMaster.Data.RepositoryInterfaces
{
    public interface IRateTrendsRepository : IGetDataTableRepository<CurrencyRateEntity, string>
    {
        IEnumerable<RateHistoryEntity> GetHistoricalRatesFor(string currencyFrom, string currencyTo, DateTime dateFrom, DateTime dateTo);
    }
}
