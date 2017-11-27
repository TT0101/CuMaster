using CuMaster.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.Data.Entities;

namespace CuMaster.Data.MockRepositories
{
    public class CurrencyRateRepository : MockRepositoryBase<CurrencyRateEntity, int>, ICurrencyRateRepository
    {
        public CurrencyRateRepository()
        {
            this.MockData.Add(new CurrencyRateEntity
            {
                 FromCurrency = "USD"
                , ToCurrency = "ISK"
                , Rate = 105.49M
                , LastUpdated = DateTime.Now
            });

            this.MockData.Add(new CurrencyRateEntity
            {
                FromCurrency = "ISK"
               ,ToCurrency = "USD"
               ,Rate = 0.01M
               ,LastUpdated = DateTime.Now
            });
        }

        public CurrencyRateEntity GetSingle(string fromCurrencyCd, string toCurrencyCd)
        {
            return this.Get().SingleOrDefault(cr => cr.FromCurrency == fromCurrencyCd && cr.ToCurrency == toCurrencyCd);
        }

        public IEnumerable<CurrencyRateEntity> GetRatesForCurrency(string currencyCd)
        {
            return this.Get().Where(cr => cr.FromCurrency == currencyCd || cr.ToCurrency == currencyCd);
        }

      

      
    }
}
