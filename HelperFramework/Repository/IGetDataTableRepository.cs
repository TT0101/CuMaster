using HelperFramework.UI.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.Repository
{
    public interface IGetDataTableRepository<T, IID>
    {
        DataTableObject<IEnumerable<T>> GetForDataTable(IID id, DataTableParams param);
    }
}
