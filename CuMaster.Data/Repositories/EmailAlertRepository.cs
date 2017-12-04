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
    public class EmailAlertRepository : IEmailAlertRepository
    {
        public void Delete(EmailAlertEntity item)
        {
            this.Delete(item.AlertID);
        }

        public void Delete(int id)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("AlertID", id);

                context.ExecuteNonResultSproc("usp_DeleteEmailAlert", sparams);
            }
        }

        public void DeleteAllForEmail(string email)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("Email", SqlDbType.VarChar);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = email;

                context.ExecuteNonResultSproc("usp_DeleteAllAlertsByEmail", sparams);
            }
        }

        public void DeleteAllForUser(string userName)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("UserName", SqlDbType.VarChar);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = userName;

                context.ExecuteNonResultSproc("usp_DeleteAllAlertsByUser", sparams);
            }
        }

        public EmailAlertEntity GetAlert(int alertID)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("AlertID", SqlDbType.Int);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = alertID;

                return context.ExecuteSproc<EmailAlertEntity>("usp_GetEmailAlert", sparams).FirstOrDefault();
            }
        }

        public DataTableObject<IEnumerable<EmailAlertRecordEntity>> GetForDataTable(string id, DataTableParams param)
        {
            DataTableObject<IEnumerable<Data.Entities.EmailAlertRecordEntity>> dto;

            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                OrderGridObject orderToUse = param.order.FirstOrDefault();

                SqlParameter[] sparams = new SqlParameter[6];
                sparams[0] = new SqlParameter("UserID", SqlDbType.VarChar);
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

                dto = context.WorkWithMultipleResultSetSproc<DataTableObject<IEnumerable<Data.Entities.EmailAlertRecordEntity>>>("usp_GetEmailAlertsForUser", sparams).FirstOrDefault();
                dto.data = context.WorkWithMultipleResultSetSproc<EmailAlertRecordEntity>("usp_GetEmailAlertsForUser", sparams);
            }

            return dto;
        }

        public void Save(EmailAlertEntity item)
        {
            string threshold;
            if(item.Threshold == 0)
            {
                threshold = null;
            }
            else
            {
                threshold = item.Threshold.ToString();
            }

            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[6];
                sparams[0] = new SqlParameter("Email", item.Email);
                sparams[1] = new SqlParameter("CurrencyFrom", item.CurrencyFrom);
                sparams[2] = new SqlParameter("CurrencyTo", item.CurrencyTo);
                if (item.TimeToSend.Value.TotalMinutes == 0)
                {
                    sparams[3] = new SqlParameter("Threshold", threshold);
                }
                else
                {
                    sparams[3] = new SqlParameter("TimetoSend", item.TimeToSend);
                }
                
                sparams[4] = new SqlParameter("SessionID", item.SessionID);
                sparams[5] = new SqlParameter("AlertID", item.AlertID);

                context.ExecuteNonResultSproc("usp_SaveEmailAlert", sparams);
            }
        }
    }
}
