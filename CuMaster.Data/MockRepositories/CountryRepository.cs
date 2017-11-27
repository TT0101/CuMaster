using CuMaster.Data.Entities;
using CuMaster.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.MockRepositories
{
    public class CountryRepository : MockRepositoryBase<CountryEntity, string>, ICountryRepository
    {
        public CountryRepository(): base()
        {
            this.MockData.Add(new CountryEntity
            {
                Code = "USA"
                ,Name = "United States"
                ,Active = true
            });

            this.MockData.Add(new CountryEntity
            {
                  Code = "ISL"
                , Name = "Iceland"
                , Active = true
            });
        }

        public CountryEntity GetSingle(string countryCd)
        {
            return this.Get().SingleOrDefault(c => c.Code == countryCd);
        }
    }
}
