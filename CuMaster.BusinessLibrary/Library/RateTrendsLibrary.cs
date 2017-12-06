using CuMaster.BusinessLibrary.Classes.Session;
using CuMaster.BusinessLibrary.ViewModels;
using HelperFramework.UI.DataTables;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CuMaster.BusinessLibrary.Library
{
    public class RateTrendsLibrary
    {

        public ViewModels.RateTrendsViewModel GetInitialRateSettings(Session session)
        {
            CurrencyLibrary cLib = new CurrencyLibrary();

            return new ViewModels.RateTrendsViewModel
            {
                BaseCurrency = session.Defaults.DefaultCurrencyFrom,
                Currencies = cLib.CurrencyFromSelect,
                LengthSelected = 1 
            };
        }

        public ViewModels.RateListViewModel GetInitialRateListSettings(Session session)
        {
            return new RateListViewModel
            {
                BaseCurrency = session.Defaults.DefaultCurrencyFrom
            };
        }

        public Dictionary<string, string> GetHistoricalRates(string currencyFrom, string currencyTo, int days)
        {
            DateTime dateTo = DateTime.Now.ToUniversalTime();
            DateTime dateFrom = dateTo.AddDays(days * -1);
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IRateTrendsRepository>();
            Dictionary<string, string> rates = new Dictionary<string, string>();
            IEnumerable<Data.Entities.RateHistoryEntity> dataRates = res.GetHistoricalRatesFor(currencyFrom, currencyTo, dateFrom, dateTo);

            return dataRates.ToDictionary(r => r.RateDate.ToString(), r => r.Rate.ToString());
        }

        public DataTableObject<IEnumerable<ViewModels.RateListViewModel>> GetRatesForBase(DataTableParams p, string baseCurrency)
        {
            Lookups.CurrencyLookup c = new Lookups.CurrencyLookup();
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IRateTrendsRepository>();
            DataTableObject<IEnumerable<Data.Entities.CurrencyRateEntity>> result = res.GetForDataTable(baseCurrency, p);
            return new DataTableObject<IEnumerable<ViewModels.RateListViewModel>>
            {
                data = ((IEnumerable<Data.Entities.CurrencyRateEntity>)result.data).Select(t => new RateListViewModel
                {
                    BaseCurrency = t.FromCurrency,
                    CurrencyTo = t.ToCurrency,
                    CurrencyToName = t.ToCurrencyName,
                    DateUpdated = t.LastUpdated,
                    RateFowards = t.Rate,
                    RateInverse = (t.Rate != 0) ? (1 / t.Rate) : 0,
                    IsFromRateCrypto = t.IsCryptoFrom,
                    IsToRateCrypto = t.IsCryptoTo
                }),
                extraData = result.extraData,
                recordsFiltered = result.recordsFiltered,
                recordsTotal = result.recordsTotal,
                draw = result.draw
            };
        }
    }
}
