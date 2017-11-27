using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.UpdateCurrencyRates
{
    public class CurrencyRatesTableTypeCollection : List<APICurrencyRates>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlMetaData[] mda = new SqlMetaData[4];
            mda[0] = new SqlMetaData("CurrencyFrom", System.Data.SqlDbType.VarChar, 10);
            mda[1] = new SqlMetaData("CurrencyTo", System.Data.SqlDbType.VarChar, 10);
            mda[2] = new SqlMetaData("Rate", System.Data.SqlDbType.VarChar, 50);
            mda[3] = new SqlMetaData("TimeUpdated", System.Data.SqlDbType.DateTime);
            
            SqlDataRecord ret = new SqlDataRecord(mda);

            foreach (APICurrencyRates data in this)
            {
                ret.SetString(0, data.CurrencyCdFrom);
                ret.SetString(1, data.CurrencyCdTo);
                ret.SetString(2, Decimal.Round(data.Rate, 9).ToString()); //this has to be a string because this thing is stupid and even with setting precision, somewhere between sending this over and the table type getting it causes it to round to a whole number
                ret.SetDateTime(3, data.TimeUpdated);
                yield return ret;
            }
        }
    }
}
