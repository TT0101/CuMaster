using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.Lookup
{
    public abstract class DataMappingLookupBase<TME, T1, T2, TID1, TID2> : LookupCacheBase<TME> 
        where TME : class 
        where T1 : class 
        where T2 : class
    {

       
        public abstract IEnumerable<T2> GetForFirstKey(TID1 id);
        public abstract IEnumerable<T1> GetForSecondKey(TID2 id);
        
    }
}
