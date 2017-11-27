using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.Repository
{
    public interface IDeleteRepository<T, TID>
    {
        void Delete(T item);

        void Delete(TID id);
    }
}
