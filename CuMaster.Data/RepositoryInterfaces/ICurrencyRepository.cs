using CuMaster.Data.Entities;
using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuMaster.Data.RepositoryInterfaces
{
    public interface ICurrencyRepository : IRepository<CurrencyEntity, string>
    {
        Dictionary<string, CurrencyEntity> GetCurrencySet(string fromCurrencyID, string toCurrencyID);
        IEnumerable<CurrencyEntity> GetActiveCurrenciesWithCurrentRates();
    }
}
