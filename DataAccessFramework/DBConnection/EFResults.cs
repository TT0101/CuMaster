using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFramework.DBConnection
{
    internal class EFResults
    {
        private DbDataReader Reader { get; set; }

        internal DbCommand CommandUsed { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        internal EFResults(DbCommand command, bool results)
        {

            this.Init(command, results);
            
        }

        internal EFResults(DbCommand command, bool results, bool doNotHoldForNonQuery)
        {
            if(!doNotHoldForNonQuery)
            {
                this.Init(command, results);
            }
            else
            {
                command.ExecuteNonQueryAsync();
                this.CommandUsed = command;
            }
        }

        private void Init(DbCommand command, bool results)
        {
            if (results)
            {
                this.Reader = this.ExecuteForDataReader(command);
            }
            else
            {
                this.ExecuteNonResult(command);
            }

            this.CommandUsed = command;
        }

        private void ExecuteNonResult(DbCommand command)
        {
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private DbDataReader ExecuteForDataReader(DbCommand command)
        {
            try
            {
                return command.ExecuteReader();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adapter"></param>
        /// <returns></returns>
        internal IEnumerable<T> GetNextResultFromDataReader<T>(IObjectContextAdapter adapter)
        {
            try
            {
                var result = adapter
                    .ObjectContext
                    .Translate<T>(this.Reader)
                    .ToList();
                this.Reader.NextResult();

                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}

