using HelperFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.MockRepositories
{
    public class MockRepositoryBase<T, TID>
    {
        internal List<T> MockData { get; set; }

        public MockRepositoryBase()
        {
                this.MockData = new List<T>();
        }

        public IEnumerable<T> Get()
        {
            return this.MockData;
        }
    }
}
