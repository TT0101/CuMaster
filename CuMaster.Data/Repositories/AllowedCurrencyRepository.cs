using CuMaster.Data.RepositoryInterfaces;
using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.Data.Entities;
using System.Data.SqlClient;
using HelperFramework.Configuration;
using System.Data;

namespace CuMaster.Data.Repositories
{
    public class AllowedCurrencyRepository : IAllowedCurrencyRepository
    {
        public IEnumerable<CurrencyEntity> GetFor(string forParameter)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("CurrencySelected", forParameter);

                return context.ExecuteSproc<CurrencyEntity>("usp_GetAllowedCurrencies", sparams);
            }
        }
    }
}
