using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.UI.DataTables
{
    public class DataTableParams
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public Collection<OrderGridObject> order { get; set; }
        public string orderColDataName { get; set; }
        public SearchGridObject search { get; set; }
        public Collection<ColumnGridObject> cols { get; set; }

        public DataTableParams()
        {

        }
    }
}
