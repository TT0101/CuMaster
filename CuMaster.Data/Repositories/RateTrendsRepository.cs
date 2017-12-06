using CuMaster.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.Data.Entities;
using HelperFramework.UI.DataTables;
using HelperFramework.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CuMaster.Data.Repositories
{
    public class RateTrendsRepository : IRateTrendsRepository
    {
        public DataTableObject<IEnumerable<CurrencyRateEntity>> GetForDataTable(string id, DataTableParams param)
        {
            DataTableObject<IEnumerable<Data.Entities.CurrencyRateEntity>> dto;

            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                OrderGridObject orderToUse = param.order.FirstOrDefault();

                SqlParameter[] sparams = new SqlParameter[6];
                sparams[0] = new SqlParameter("BaseCurrency", SqlDbType.VarChar);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = id;

                sparams[1] = new SqlParameter("Start", param.start);
                sparams[1].SqlDbType = SqlDbType.Int;

                sparams[2] = new SqlParameter("Length", param.length);
                sparams[2].SqlDbType = SqlDbType.Int;

                sparams[3] = new SqlParameter("OrderByCol", (orderToUse == null) ? "" : param.cols.ElementAt(orderToUse.column).name);
                sparams[3].SqlDbType = SqlDbType.VarChar;

                sparams[4] = new SqlParameter("OrderDirection", (orderToUse == null) ? "" : orderToUse.dir);
                sparams[4].SqlDbType = SqlDbType.VarChar;

                sparams[5] = new SqlParameter("SearchText", (param.search == null) ? "" : (param.search.value ?? ""));
                sparams[5].SqlDbType = SqlDbType.VarChar;

                dto = context.WorkWithMultipleResultSetSproc<DataTableObject<IEnumerable<Data.Entities.CurrencyRateEntity>>>("usp_GetCurrencyRatesForBaseTable", sparams).FirstOrDefault();
                dto.data = context.WorkWithMultipleResultSetSproc<CurrencyRateEntity>("usp_GetCurrencyRatesForBaseTable", sparams);
                dto.draw = param.draw;
            }

            return dto;
        }

        public IEnumerable<RateHistoryEntity> GetHistoricalRatesFor(string currencyFrom, string currencyTo, DateTime dateFrom, DateTime dateTo)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[4];
                sparams[0] = new SqlParameter("CurrencyFrom", currencyFrom);
                sparams[1] = new SqlParameter("CurrencyTo", currencyTo);
                sparams[2] = new SqlParameter("DateFrom", dateFrom);
                sparams[3] = new SqlParameter("DateTo", dateTo);

                return context.ExecuteSproc<RateHistoryEntity>("usp_GetHistoricalCurrencyRates", sparams);
            }
        }
    }
}
