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
    public class CurrencyRateRepository : RepositoryBase<CurrencyRateEntity>, ICurrencyRateRepository
    {
        public CurrencyRateEntity GetSingle(string fromCurrencyCd, string toCurrencyCd)
        {
            return this.EntityData.SingleOrDefault(e => e.FromCurrency == fromCurrencyCd && e.ToCurrency == toCurrencyCd);
        }
      
        public IEnumerable<CurrencyRateEntity> GetRatesForCurrency(string currencyCd)
        {
            return this.EntityData.Where(e => e.FromCurrency == currencyCd || e.ToCurrency == currencyCd);
        }

        internal override void RefreshData()
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                this.EntityData = context.ExecuteSproc<CurrencyRateEntity>("usp_GetCurrencyRates");
            }
        }
    }
}
