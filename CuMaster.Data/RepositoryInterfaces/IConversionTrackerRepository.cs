using CuMaster.Data.Entities;
using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.RepositoryInterfaces
{
    public interface IConversionTrackerRepository : IGetDataTableRepository<ConversionTrackerEntity, string>, IEditRepository<ConversionTrackerEntity, int>, IDeleteRepository<ConversionTrackerEntity, int>
    {
        void SaveExisting(int entryID);
        void DeleteAll(string sessionID);
    }
}
