using CuMaster.BusinessLibrary.Models;
using HelperFramework.Lookup;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CuMaster.BusinessLibrary.Lookups
{
    public class AllowedConversionsLookup : LookupCacheBase<CurrencyRateModel>
    {
        public AllowedConversionsLookup() : base(1) //aka I don't want this caching for much longer than the updates, just in case something is older
        {

        }

        public IEnumerable<string> GetCurrenciesAllowedToPair(string fromCurrencyCd)
        {
            return base.Data.Where(cr => cr.FromCurrency.ID == fromCurrencyCd).Select(cr => cr.ToCurrency.ID);
        }

        public CurrencyRateModel GetRatePair(string fromCurrencyCd, string toCurrencyCd)
        {
            return base.Data.SingleOrDefault(cr => cr.FromCurrency.ID == fromCurrencyCd && cr.ToCurrency.ID == toCurrencyCd);
        }
       
        protected override IEnumerable<CurrencyRateModel> RefreshData()
        {
            var cRes = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.ICurrencyRateRepository>();
            IEnumerable<CurrencyRateModel> cml = cRes.Get().Select(c => new CurrencyRateModel(c));

            return cml;
        }
    }
}
