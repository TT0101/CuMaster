using CuMaster.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using CuMaster.Data.Entities;
using HelperFramework.Configuration;
using System.Linq;

namespace CuMaster.Data.Repositories
{
    public class CountryRepository : RepositoryBase<CountryEntity>, ICountryRepository
    {
        public CountryRepository() : base()
        {

        }

        internal override void RefreshData()
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                this.EntityData = context.ExecuteSproc<CountryEntity>("usp_GetCountries");
            }
        }

        public CountryEntity GetSingle(string id)
        {
            return this.EntityData.SingleOrDefault(c => c.Code == id);
        }
    }
}
