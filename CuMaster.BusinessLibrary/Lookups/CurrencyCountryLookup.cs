using CuMaster.BusinessLibrary.Models;
using CuMaster.Data.Entities;
using HelperFramework.Lookup;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.Lookups
{
    public class CurrencyCountryLookup : DataMappingLookupBase<CurrencyCountryEntity, CurrencyModel, CountryModel, string, string>
    {
        public override IEnumerable<CountryModel> GetForFirstKey(string currencyCd)
        {
            IEnumerable<CurrencyCountryEntity> entities =  this.Data.Where(cc => cc.CurrencyCd == currencyCd);
            CountryLookup clook = new CountryLookup();
            return entities.Select(cc => clook.GetItemFromLookup(cc.CountryCd));
        }

        public override IEnumerable<CurrencyModel> GetForSecondKey(string countryCd)
        {
            IEnumerable<CurrencyCountryEntity> entities = this.Data.Where(cc => cc.CountryCd == countryCd);
            CurrencyLookup clook = new CurrencyLookup(true);
            return entities.Select(cc => clook.GetItemFromLookup(cc.CurrencyCd));
        }

        protected override IEnumerable<CurrencyCountryEntity> RefreshData()
        {
            var cRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.ICurrencyCountryRepository>();
            IEnumerable<CurrencyCountryEntity> cml = cRes.Get().Where(c => c.Active);

            return cml;
        }
    }
}
