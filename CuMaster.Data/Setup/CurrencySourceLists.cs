using HelperFramework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.Setup
{
    internal class CurrencySourceLists
    {
        public IEnumerable<CurrencySource> FullCurrencyList { get; set; }
        public bool ActiveOnly { get; set; }
        internal CurrencySourceLists(bool activeOnly)
        {
            this.ActiveOnly = activeOnly;
            //get full list from db
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                this.FullCurrencyList = context.ExecuteSproc<CurrencySource>("usp_GetCurrencySources").Where(c => c.Active = (this.ActiveOnly) ? true : c.Active);
            }
        }

        internal IEnumerable<CurrencySource> GetListOfToCurrencies(string fromSource)
        {
            return this.FullCurrencyList.Where(c => c.SourceTo == fromSource || c.SourceTo == "BOTH");
        }

    }
}
