using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.UI.DataTables
{
    public class ColumnGridObject
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool orderable { get; set; }
        public SearchGridObject search { get; set; }
        public bool searchable { get; set; }

        public ColumnGridObject()
        {

        }

    }
}
