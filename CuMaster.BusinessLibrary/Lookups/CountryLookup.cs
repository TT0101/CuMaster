using CuMaster.BusinessLibrary.Models;
using HelperFramework.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using System.Web.Mvc;

namespace CuMaster.BusinessLibrary.Lookups
{
    public class CountryLookup : LookupBase<CountryModel, string>
    {
        public override Dictionary<string, CountryModel> GetLookupDictionary()
        {
            return base.Data.ToDictionary(c => c.Code, c => c);
        }

        public override Dictionary<string, string> GetNameValueDictionary()
        {
            return base.Data.ToDictionary(c => c.Code, c => c.Name);
        }

        public override IList<SelectListItem> GetSelectList()
        {
            return base.Data.Select(c => new SelectListItem { Value = c.Code, Text = c.Name }).ToList();
        }

        protected override IEnumerable<CountryModel> RefreshData()
        {
            var cRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.ICountryRepository>();
            IEnumerable<CountryModel> cml = cRes.Get().Where(c => (base.GetActiveOnly) ? true : c.Active).Select(c => new CountryModel(c));

            return cml;
        }

    }
}
