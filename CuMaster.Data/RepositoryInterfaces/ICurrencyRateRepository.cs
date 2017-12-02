using CuMaster.Data.Entities;
using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuMaster.Data.RepositoryInterfaces
{
    public interface ICurrencyRateRepository : IGetRepository<CurrencyRateEntity>
    {
        IEnumerable<CurrencyRateEntity> GetRatesForCurrency(string currencyCd, bool fromToOnly);
        CurrencyRateEntity GetSingle(string fromCurrencyCd, string toCurrencyCd);


    }
}
