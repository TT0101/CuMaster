using CuMaster.BusinessLibrary.Models;
using HelperFramework.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Web.Mvc;

namespace CuMaster.BusinessLibrary.Lookups
{
    public class CurrencyLookup : LookupBase<CurrencyModel, string>
    {
        public CurrencyLookup() :base()
        {

        }

        public CurrencyLookup(bool activeOnly) : base(activeOnly)
        {
        }

        public override Dictionary<string, CurrencyModel> GetLookupDictionary()
        {
            return base.Data.ToDictionary(c => c.ID, c => c);
        }

        public override Dictionary<string, string> GetNameValueDictionary()
        {
            return base.Data.ToDictionary(c => c.ID, c => c.Name);
        }

        public override IList<SelectListItem> GetSelectList()
        {
            return base.Data.Select(c => new SelectListItem { Value = c.ID, Text = c.Name }).ToList();
        }

        public IEnumerable<CurrencyModel> GetSelectList(IEnumerable<string> allowedValues)
        {
            return base.Data.Join(allowedValues, c => c.ID, a => a, (c,a) => c);
        }

        protected override IEnumerable<CurrencyModel> RefreshData()
        {
            var cRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.ICurrencyRepository>();
            
            IEnumerable<CurrencyModel> cMs = cRes.Get().Where(c => (base.GetActiveOnly) ? true: c.Active).Select(c => new CurrencyModel(c));

            return cMs;
        }

        private List<CountryModel> GetCountryInfo(List<string> countryCds)
        {
            List<CountryModel> countryList = new List<CountryModel>();
            var coLookup = new CountryLookup();

            foreach (string cCode in countryCds)
            {
                countryList.Add(coLookup.GetItemFromLookup(cCode));
            }

            return countryList;
        }
    }
}
