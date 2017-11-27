using HelperFramework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HelperFramework.Lookup
{
    public abstract class LookupBase<T, TID> : LookupCacheBase<T> where T : class
    {
        protected bool GetActiveOnly { get; set; }

        public LookupBase() : base()
        {
            this.GetActiveOnly = false;
        }

        public LookupBase(bool activeOnly) : base()
        {
            this.GetActiveOnly = activeOnly;
        }

        public LookupBase(int hoursToCache) : base(hoursToCache)
        {

        }

        public T GetItemFromLookup(TID key)
        {
            if (this.GetLookupDictionary().ContainsKey(key))
                return this.GetLookupDictionary()[key];
            else
                return default(T);
        }

        public abstract Dictionary<string, string> GetNameValueDictionary();

        public abstract IList<SelectListItem> GetSelectList();

        public abstract Dictionary<TID, T> GetLookupDictionary();


    }
}
