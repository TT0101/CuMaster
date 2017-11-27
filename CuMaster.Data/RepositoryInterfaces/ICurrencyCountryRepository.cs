using CuMaster.Data.Entities;
using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.RepositoryInterfaces
{
    public interface ICurrencyCountryRepository : IGetRepository<CurrencyCountryEntity>
    {
        CurrencyCountryEntity GetSingle(string currencyCd, string countryCd);
        IEnumerable<CurrencyCountryEntity> GetForCurrency(string currencyCd);
        IEnumerable<CurrencyCountryEntity> GetForCountry(string countryCd);
    }
}
