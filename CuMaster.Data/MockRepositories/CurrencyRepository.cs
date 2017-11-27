using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.Data.Entities;

namespace CuMaster.Data.MockRepositories
{
    public class CurrencyRepository : MockRepositoryBase<CurrencyEntity, string>//, RepositoryInterfaces.ICurrencyRepository
    {
        public CurrencyRepository() : base()
        {
            List<string> country = new List<string>();
            country.Add("USA");

            this.MockData.Add(new CurrencyEntity
            {
                ID = "USD",
                Name = "US Dollar",
                ASCIISymbol = "36",
                HTMLSymbol = "&#0036;",
                Active = true
            });

            country.Clear();
            country.Add("ISL");

            this.MockData.Add(new CurrencyEntity
            {
                ID = "ISK",
                Name = "Icelandic Krona",
                ASCIISymbol = "107,114",
                HTMLSymbol = "kr",
                Active = true
            });
        }

        public IEnumerable<CurrencyEntity> GetActiveCurrenciesWithCurrentRates()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, CurrencyEntity> GetCurrencySet(string fromCurrencyID, string toCurrencyID)
        {
            return this.Get().Where(c => c.ID == fromCurrencyID || c.ID == toCurrencyID).ToDictionary(c => c.ID, c => c);
        }

        public CurrencyEntity GetSingle(string currencyID)
        {
            return this.Get().SingleOrDefault(c => c.ID == currencyID);
        }
    }
}
