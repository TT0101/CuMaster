using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.Repositories
{
    public abstract class RepositoryBase<T> : IGetRepository<T>
    {
        public IEnumerable<T> EntityData { get; set; }

        public RepositoryBase()
        {
            this.RefreshData();
        }

        public IEnumerable<T> Get()
        {
            return this.EntityData;
        }

        internal abstract void RefreshData();
    }
}
