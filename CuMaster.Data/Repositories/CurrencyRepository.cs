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
    public class CurrencyRepository : RepositoryBase<CurrencyEntity>, ICurrencyRepository
    {
        public CurrencyRepository() : base()
        {
            
        }

        internal override void RefreshData()
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                this.EntityData = context.ExecuteSproc<CurrencyEntity>("usp_GetCurrencies");
            }
        }

        public Dictionary<string, CurrencyEntity> GetCurrencySet(string fromCurrencyID, string toCurrencyID)
        {
            return this.EntityData.Where(c => c.ID == fromCurrencyID || c.ID == toCurrencyID).ToDictionary(c => c.ID, c => c);
        }

        public CurrencyEntity GetSingle(string id)
        {
            return this.EntityData.SingleOrDefault(c => c.ID == id);
        }

        public IEnumerable<CurrencyEntity> GetActiveCurrenciesWithCurrentRates()
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                return context.ExecuteSproc<CurrencyEntity>("usp_GetCurrenciesWithCurrentRates");
            }
        }
    }
}
