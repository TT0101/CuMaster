using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HelperFramework.Configuration;
using System.Data.Entity.SqlServer;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace DataAccessFramework.DBConnection
{
    public class DBConnectionContext : DbContext
    {
        private EFResults efr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName"></param>
        public DBConnectionContext(DatabaseName dbName) : base(ConnectionStringManager.GetConnectionString(dbName))
        {
            this.InitDLLs();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        protected DBConnectionContext(string connectionString) : base(connectionString)
        {
            this.InitDLLs();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InitDLLs()
        {
            var dllSQL = SqlProviderServices.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        public void ExecuteNonResultSproc(string sprocName, params DbParameter[] parameters)
        {
            new EFResults(this.SetUpCommandObjectForSproc(sprocName, parameters), false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        public void ExecuteNonResultSprocBegin(string sprocName, params DbParameter[] parameters)
        {
            new EFResults(this.SetUpCommandObjectForSproc(sprocName, parameters), false, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteSproc<T>(string sprocName, params DbParameter[] parameters)
        {
            this.efr = new EFResults(this.SetUpCommandObjectForSproc(sprocName, parameters), true);
            return efr.GetNextResultFromDataReader<T>((IObjectContextAdapter)this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> WorkWithMultipleResultSetSproc<T>(string sprocName, params DbParameter[] parameters)
        {
            if (efr == null || efr.CommandUsed.CommandText != sprocName)
            {
                return this.ExecuteSproc<T>(sprocName, parameters);
            }
            else
            {
                return efr.GetNextResultFromDataReader<T>((IObjectContextAdapter)this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TResult ExecuteScalarSproc<TResult>(string sprocName, params DbParameter[] parameters)
        {
            DbCommand command = this.SetUpCommandObjectForSproc(sprocName, parameters);

            return (TResult)command.ExecuteScalar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DbCommand SetUpCommandObjectForSproc(string sprocName, params DbParameter[] parameters)
        {
            if (this.Database.Connection.State == System.Data.ConnectionState.Closed)
            {
                this.Database.Connection.Open();
            }

            DbCommand command = this.Database.Connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = sprocName;

            if (parameters != null && parameters.Length > 0)
            {
               command.Parameters.AddRange(parameters);
            }

            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Dispose()
        {
            this.Database.Connection.Close();
            efr = null;
            base.Dispose();
        }
    }
}
