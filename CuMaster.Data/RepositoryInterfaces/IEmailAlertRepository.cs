using CuMaster.Data.Entities;
using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.RepositoryInterfaces
{
    public interface IEmailAlertRepository : IGetDataTableRepository<EmailAlertEntity, string>, IEditRepository<EmailAlertEntity, int>, IDeleteRepository<EmailAlertEntity, int>
    {
        void DeleteAllForEmail(string email);
    }
}
