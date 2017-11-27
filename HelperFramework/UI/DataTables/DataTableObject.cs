using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.UI.DataTables
{
    //public class DataTableObject<T> where T: new()
    //{
    //    public static DataTableObject Create()
    //    {
    //        return new DataTableObject(typeof(T));
    //    }
    //}

    public class DataTableObject<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public Object data { get; set; }
        public Object extraData { get; set; }

        //public DataTableObject(Type dataType)
        //{
        //    this.data = Activator.CreateInstance(dataType);
        //}

        public DataTableObject()
        {
            data = new T[0];
            recordsFiltered = 0;
            recordsTotal = 0;
            draw = 0;
        }
    }
}
