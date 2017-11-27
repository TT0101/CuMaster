using CuMaster.Data.Entities;
using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuMaster.Data.RepositoryInterfaces
{
    public interface ICountryRepository : IRepository<CountryEntity, string>
    {
    }
}
