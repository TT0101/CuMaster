using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using CuMaster.Data;

namespace DIResolver.Data
{
    public class RepositoryResolver : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(CuMaster.Data.RepositoryInterfaces.ICountryRepository)).To(typeof(CuMaster.Data.Repositories.CountryRepository));
            Bind(typeof(CuMaster.Data.RepositoryInterfaces.ICurrencyRepository)).To(typeof(CuMaster.Data.Repositories.CurrencyRepository));
            Bind(typeof(CuMaster.Data.RepositoryInterfaces.ICurrencyRateRepository)).To(typeof(CuMaster.Data.Repositories.CurrencyRateRepository));
            Bind(typeof(CuMaster.Data.RepositoryInterfaces.ICurrencyCountryRepository)).To(typeof(CuMaster.Data.Repositories.CountryCurrencyRepository));
            Bind(typeof(CuMaster.Data.RepositoryInterfaces.IConversionTrackerRepository)).To(typeof(CuMaster.Data.Repositories.ConversionTrackerRepository));
            Bind(typeof(CuMaster.Data.RepositoryInterfaces.IAllowedCurrencyRepository)).To(typeof(CuMaster.Data.Repositories.AllowedCurrencyRepository));
            Bind(typeof(CuMaster.Data.RepositoryInterfaces.IEmailAlertRepository)).To(typeof(CuMaster.Data.Repositories.EmailAlertRepository));
        }
       
    }
}
