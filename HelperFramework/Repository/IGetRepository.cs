using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.Repository
{
    public interface IGetRepository<T>
    {
        IEnumerable<T> Get();
    }
}
