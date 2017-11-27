using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.Setup
{
    public static class CurrencyRateRetrieval
    {
        public enum Sources
        {
            Yahoo,
            Cryptonator
        }

        static string[] SourceCodes = { "YAHOO", "CRYPTO" };
        static string[] SourceDelimiters = { "", "-" };

        public static void LoadCurrencyRates()
        {
            List<APICurrencyRates> rList = new List<APICurrencyRates>();
            for (int i = 0; i < SourceCodes.Length; i++)
            {
                rList.AddRange(RetrieveCurrencyRates((Sources)i));
            }

            SaveRates(rList);
        }

        public static void LoadCurrencyRates(Sources source)
        {
            List<APICurrencyRates> rList = RetrieveCurrencyRates(source);
            SaveRates(rList);
        }

        private static List<APICurrencyRates> RetrieveCurrencyRates(Sources source)
        {
            List<APICurrencyRates> pairs = CreateCurrencyPairsForAPICall(source);
            GetRatesFromAPI(ref pairs, source); //or swap with generate random ones
            return pairs;
        }

        private static List<APICurrencyRates> CreateCurrencyPairsForAPICall(Sources source)
        {
            CurrencySourceLists sourceList = new CurrencySourceLists(true);
            IEnumerable<string> toRates;
            List<APICurrencyRates> rateList = new List<APICurrencyRates>();

            foreach(CurrencySource cs in sourceList.FullCurrencyList)
            {
                toRates = sourceList.GetListOfToCurrencies(cs.SourceFrom).Select(c => c.CurrencyCd);
                foreach(string cd in toRates)
                {
                    if(!rateList.Any(cr => (cr.CurrencyCdFrom == cs.CurrencyCd && cr.CurrencyCdTo == cd) || (cr.CurrencyCdFrom == cd && cr.CurrencyCdTo == cs.CurrencyCd)))
                    {
                        APICurrencyRates acr = new APICurrencyRates(cs.CurrencyCd, cd, SourceDelimiters[(int)source]);
                        rateList.Add(acr);
                    }
                }
            }

            return rateList;
        }

        private static void CalculateOppositePairsAndAddToList(ref List<APICurrencyRates> rates)
        {
            foreach (APICurrencyRates cr in rates.ToList())
            {
                if (!rates.Any(r => r.CurrencyCdFrom == cr.CurrencyCdTo && r.CurrencyCdTo == cr.CurrencyCdFrom))
                {
                    APICurrencyRates ocr = new APICurrencyRates(cr.CurrencyCdTo, cr.CurrencyCdFrom, cr.Delimiter)
                    {
                        Rate = 1 / cr.Rate
                    };
                    rates.Add(ocr);
                }
            }
        }

        private static void GetRatesFromAPI(ref List<APICurrencyRates> rates, Sources source)
        {
            List<APICurrencyRates> ratesToCycle = rates.ToList();
            foreach(APICurrencyRates r in ratesToCycle)
            {
                APICurrencyRates actualRate = rates.SingleOrDefault(ar => ar.CurrencyCdFrom == r.CurrencyCdFrom && ar.CurrencyCdTo == r.CurrencyCdTo);
                if (actualRate != null && actualRate.Rate == 0) //if we haven't determined a rate for this yet
                {
                    //get rate from api 
                    if (source == Sources.Yahoo)
                    {
                        APIHandlers.YahooAPIHandler yahooAPI = new APIHandlers.YahooAPIHandler();
                        actualRate.Rate = yahooAPI.GetRate(r.CurrencyCdFrom, r.CurrencyCdTo);
                    }
                    else if (source == Sources.Cryptonator)
                    {

                    }
                    else
                        throw new NotSupportedException("The source provided is not a supported currency source.");

                    //then see if we can calculate any from this call using cross rates (requires at least two mappings with the same from currency) This keeps the api calls down
                    if(rates.Any(ra => ra.CurrencyCdFrom == r.CurrencyCdFrom && ra.Rate > 0))
                    { 
                        List<APICurrencyRates> otherRatesFound = rates.Where(r2 => r2.CurrencyCdFrom == r.CurrencyCdFrom && r2.Rate != 0).ToList();
                        IEnumerable<APICurrencyRates> ratesToCalc = rates.Where(r3 => otherRatesFound.Any(or => or.CurrencyCdTo == r3.CurrencyCdFrom) && r3.Rate == 0);
                        foreach(APICurrencyRates rc in ratesToCalc.ToList())
                        {
                            APICurrencyRates rate1 = rates.SingleOrDefault(r11 => r11.CurrencyCdFrom == r.CurrencyCdFrom && r11.CurrencyCdTo == rc.CurrencyCdTo);
                            APICurrencyRates rate2 = rates.SingleOrDefault(r12 => r12.CurrencyCdFrom == r.CurrencyCdFrom && r12.CurrencyCdTo == rc.CurrencyCdFrom);
                            if(rate1 != null && rate2 != null && rate1.Rate != 0 && rate2.Rate != 0) //if we somehow found a bad rate, don't use it
                            {
                                rc.Rate = rate1.Rate * (1 / rate2.Rate);
                            }
                        }
                    }
                }
            }
        }

        private static void GenerateRandomRates(ref List<APICurrencyRates> rates)
        {
            throw new NotImplementedException();
        }

        private static void SaveRates(List<APICurrencyRates> rates)
        {
            //for each rate listed
            //1) find if rate ID exists for the rate
            //2) find if rate ID exists for the opposite rate
        }

        private static decimal GenerateRandomRate()
        {
            Random r = new Random();
            int newRate = r.Next(0, 1000);
            double newPartRate = r.NextDouble();

            return (decimal)(newRate + newPartRate);
        }
    }
}
