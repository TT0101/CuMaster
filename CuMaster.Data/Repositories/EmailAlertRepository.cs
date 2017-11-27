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
            throw new NotImplementedException();
        }

        public void DeleteAllForEmail(string email)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("Email", SqlDbType.VarChar);
                sparams[0].Direction = System.Data.ParameterDirection.Input;
                sparams[0].Value = email;

                context.ExecuteNonResultSproc("usp_DeleteallAlertsByEmail", sparams);
            }
        }

        public DataTableObject<IEnumerable<EmailAlertEntity>> GetForDataTable(string id, DataTableParams param)
        {
            throw new NotImplementedException();
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
                sparams[3] = new SqlParameter("Threshold", threshold);
                sparams[4] = new SqlParameter("TimetoSend", item.TimeToSend);
                sparams[5] = new SqlParameter("SessionID", item.SessionID);

                context.ExecuteNonResultSproc("usp_SaveEmailAlert", sparams);
            }
        }
    }
}
