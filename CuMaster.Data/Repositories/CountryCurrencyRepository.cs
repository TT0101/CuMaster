using CuMaster.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.Data.Entities;
using HelperFramework.Configuration;

namespace CuMaster.Data.Repositories
{
    public class CountryCurrencyRepository : RepositoryBase<CurrencyCountryEntity>, ICurrencyCountryRepository 
    {
        public CountryCurrencyRepository() : base()
        {
        }

        internal override void RefreshData()
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                this.EntityData = context.ExecuteSproc<CurrencyCountryEntity>("usp_GetCurrencyCountries");
            }
        }

        public IEnumerable<CurrencyCountryEntity> GetForCountry(string countryCd)
        {
            return this.Get().Where(cc => cc.CountryCd == countryCd);
        }

        public IEnumerable<CurrencyCountryEntity> GetForCurrency(string currencyCd)
        {
            return this.Get().Where(cc => cc.CurrencyCd == currencyCd);
        }

        public CurrencyCountryEntity GetSingle(string currencyCd, string countryCd)
        {
            return this.Get().SingleOrDefault(cc => cc.CurrencyCd == currencyCd && cc.CountryCd == countryCd);
        }
    }
}
