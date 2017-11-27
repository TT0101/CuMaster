using HelperFramework.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.UpdateCurrencyRates
{
    internal class CurrencySourceLists
    {
        public IEnumerable<CurrencySource> FullCurrencyList { get; set; }
        public bool ActiveOnly { get; set; }
        internal CurrencySourceLists(bool activeOnly, string sourceFrom = "")
        {
            this.ActiveOnly = activeOnly;
            this.FullCurrencyList = GetData(sourceFrom).Where(c => c.Active = (this.ActiveOnly) ? true : c.Active);
        }

        private IEnumerable<CurrencySource> GetData(string sourceFrom)
        {
            //get full list from db
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                if (sourceFrom != "")
                {
                    SqlParameter[] sparams = new SqlParameter[1];
                    sparams[0] = new SqlParameter("SourceFrom", sourceFrom);
                    sparams[0].Direction = System.Data.ParameterDirection.Input;
                    return context.ExecuteSproc<CurrencySource>("usp_GetCurrencySources", sparams);
                }
                else
                {
                    return context.ExecuteSproc<CurrencySource>("usp_GetCurrencySources");
                }
            }
        }

        internal IEnumerable<CurrencySource> GetListOfToCurrencies(string fromSource, string baseCurrency)
        {
            return this.FullCurrencyList.Where(c => (c.SourceTo == fromSource || c.SourceTo == "BOTH") && c.CurrencyCd != baseCurrency);
        }

    }
}
