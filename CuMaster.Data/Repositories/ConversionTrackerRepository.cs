using CuMaster.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.Data.Entities;
using HelperFramework.Repository;
using System.Data.SqlClient;
using System.Data;
using HelperFramework.Configuration;
using HelperFramework.UI.DataTables;

namespace CuMaster.Data.Repositories
{
    public class ConversionTrackerRepository : IConversionTrackerRepository
    {
        public void Delete(ConversionTrackerEntity item)
        {
            this.Delete(item.EntryID);
        }

        public void Delete(int id)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("EntryID", SqlDbType.Int);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = id;

                context.ExecuteNonResultSproc("usp_DeleteTrackerEntry", sparams);
            }
        }

        public void DeleteAll(string sessionID)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("SessionID", SqlDbType.VarChar);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = sessionID;

                context.ExecuteNonResultSproc("usp_DeleteAllTrackerEntries", sparams);
            }
        }

        public DataTableObject<IEnumerable<Data.Entities.ConversionTrackerEntity>> GetForDataTable(string forParameter, DataTableParams param)
        {
            DataTableObject<IEnumerable<Data.Entities.ConversionTrackerEntity>> dto;

            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                OrderGridObject orderToUse = param.order.FirstOrDefault();

                SqlParameter[] sparams = new SqlParameter[6];
                sparams[0] = new SqlParameter("SessionID", SqlDbType.VarChar);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = forParameter;

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

                dto = context.WorkWithMultipleResultSetSproc<DataTableObject<IEnumerable<Data.Entities.ConversionTrackerEntity>>>("usp_GetTrackerEntries", sparams).FirstOrDefault();
                dto.data = context.WorkWithMultipleResultSetSproc<ConversionTrackerEntity>("usp_GetTrackerEntries", sparams);
            }

            return dto;


        }
       
        public void Save(ConversionTrackerEntity item)
        { 
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[10];
                sparams[0] = new SqlParameter("EntryName", item.EntryName);
                sparams[1] = new SqlParameter("CurrencyFrom", item.CurrencyFrom);
                sparams[2] = new SqlParameter("CurrencyTo", item.CurrencyTo);
                sparams[3] = new SqlParameter("FromAmount", item.FromAmount);
                sparams[4] = new SqlParameter("ToAmount", item.ToAmount);
                sparams[5] = new SqlParameter("RateToUse", item.RateToUse);
                sparams[6] = new SqlParameter("UpdateRate", item.UpdateRate);
                sparams[7] = new SqlParameter("LastUpdatedDate", item.LastUpdatedDate.ToUniversalTime());
                sparams[8] = new SqlParameter("SessionID", item.SessionID);
                sparams[9] = new SqlParameter("DateCookieExpires", null); //get this from cookie object using session id when built.  For now it'll work

                context.ExecuteNonResultSproc("usp_SaveNewTrackerEntry", sparams);
            }
        }

        public void SaveExisting(int entryID)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("EntryID", SqlDbType.Int);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = entryID;

                context.ExecuteNonResultSproc("usp_SaveEntryAutoUpdateChange", sparams);
            }
        }

    }
}
