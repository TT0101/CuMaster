using CuMaster.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.Data.Entities;
using HelperFramework.Configuration;
using System.Data.SqlClient;

namespace CuMaster.Data.Repositories
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        public CurrencyRateEntity GetSingle(string fromCurrencyCd, string toCurrencyCd)
        {
            //return this.EntityData.SingleOrDefault(e => e.FromCurrency == fromCurrencyCd && e.ToCurrency == toCurrencyCd);
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[2];
                sparams[0] = new SqlParameter("CurrencyFrom", fromCurrencyCd);
                sparams[1] = new SqlParameter("CurrencyTo", toCurrencyCd);

                return context.ExecuteSproc<CurrencyRateEntity>("usp_GetCurrencyRatesForPair", sparams).FirstOrDefault();
            }
        }

        public IEnumerable<CurrencyRateEntity> GetRatesForCurrency(string currencyCd, bool fromToOnly)
        {
            //return this.EntityData.Where(e => e.FromCurrency == currencyCd || e.ToCurrency == currencyCd);
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[2];
                sparams[0] = new SqlParameter("Currency", currencyCd);
                sparams[1] = new SqlParameter("GetOnlyFromTo", fromToOnly);

                return context.ExecuteSproc<CurrencyRateEntity>("usp_GetCurrencyRatesUsingCurrency", sparams);
            }
        }

        //internal override void RefreshData()
        //{
        //    using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
        //    {
        //        this.EntityData = context.ExecuteSproc<CurrencyRateEntity>("usp_GetCurrencyRates");
        //    }
        //}

        public IEnumerable<CurrencyRateEntity> Get()
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                return context.ExecuteSproc<CurrencyRateEntity>("usp_GetCurrencyRates");
            }
        }
    }
}
