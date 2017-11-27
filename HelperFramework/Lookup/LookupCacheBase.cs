using HelperFramework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.Lookup
{
    public abstract class LookupCacheBase<T>
    {
        private int CacheHours { get; set; }

        private string Key
        {
            get
            {
                return typeof(T).FullName.ToString();
            }
        }

        private CacheItemPolicy Policy
        {
            get
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(this.CacheHours);
                return policy;
            }
        }

        //private IEnumerable<T> _data;
        protected IEnumerable<T> Data
        {
            get
            {

                if (!AppCache.Contains(this.Key))
                {
                    return (IEnumerable<T>)AppCache.Insert(this.Key, this.RefreshData(), this.Policy);
                }

                return AppCache.Get<IEnumerable<T>>(this.Key);
            }
            set
            {
                AppCache.Insert(this.Key, value, this.Policy);
            }
        }

        public LookupCacheBase()
        {
            this.CacheHours = 24;
        }

        public LookupCacheBase(int hoursToCache)
        {
            //this.Data = this.RefreshData();
            this.CacheHours = hoursToCache;
        }

        protected abstract IEnumerable<T> RefreshData();


    }
}
