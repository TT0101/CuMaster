using CuMaster.Data.Entities;
using HelperFramework.Configuration;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.UpdateCurrencyRates
{
    public static class CurrencyRateRetrieval
    {
        public enum Sources
        {
            CurrencyLayer,
            Cryptonator,
            Yahoo
        }

        static string[] SourceCodes = { "LAYER", "CRYPTO", "YAHOO" };
        static string[] SourceDelimiters = { "", "-", "" };

        static bool getActiveOnly = false;

        public static void LoadCurrencyRates()
        {
            //Task[] tasks = new Task[SourceCodes.Length];
            //Parallel.For (0, SourceCodes.Length, i =>
            for (int i = 0; i < SourceCodes.Length; i++)
            {
                 LoadCurrencyRates((Sources)i);
            }//);

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
            CurrencySourceLists sourceList = new CurrencySourceLists(getActiveOnly, SourceCodes[(int)source]);
            Task<List<APICurrencyRates>> t = CreateCurrencyPairsForSourceListAsync(sourceList, sourceList.FullCurrencyList, source); //take is test code
            t.Wait(); //wait for this to create all the pairs
            return t.Result; 
        }

        private static async Task<List<APICurrencyRates>> CreateCurrencyPairsForSourceListAsync(CurrencySourceLists sourceList, IEnumerable<CurrencySource> listOfBaseRates, Sources source)
        {
            
            List<APICurrencyRates> rateList = new List<APICurrencyRates>();
            //Parallel.ForEach(listOfBaseRates, cs => 
            List<List<APICurrencyRates>> results = new List<List<APICurrencyRates>>();
            foreach (CurrencySource cs in listOfBaseRates)
            {
                results.Add(await GenerateCurrencyPairsAsync(sourceList, cs, source));
            }
           
            foreach(List<APICurrencyRates> r in results)
            {
                rateList.AddRange(r);
            }

            return rateList;
        }

        private static async Task<List<APICurrencyRates>> GenerateCurrencyPairsAsync(CurrencySourceLists sourceList, CurrencySource cs, Sources source)
        {

            IEnumerable<string> toRates;
            List<APICurrencyRates> rateList = new List<APICurrencyRates>();
            toRates = sourceList.GetListOfToCurrencies(cs.SourceFrom, cs.CurrencyCd).Select(c => c.CurrencyCd); //take is test code
             //Parallel.ForEach(toRates, cd =>
             foreach (string cd in toRates)
             {
                    APICurrencyRates acr = new APICurrencyRates(cs.CurrencyCd, cd, SourceDelimiters[(int)source]);
                    // Debug.WriteLine(acr.CurrencyPairID);
                    rateList.Add(acr);
             }//);

            return rateList;
        }

        private static void GetRatesFromAPI(ref List<APICurrencyRates> rates, Sources source)
        {
            if (rates.Any())
            {
                //get rate from api 
                if (source == Sources.CurrencyLayer)
                {
                    if (!rates.Any(r => r.CurrencyCdFrom == "USD")) //if usd isn't the list, need it to be, so create them
                    {
                        CurrencySourceLists sourceList = new CurrencySourceLists(getActiveOnly, SourceCodes[(int)source]);
                        List<CurrencySource> usdOnly = new List<CurrencySource>();
                        usdOnly.Add(new CurrencySource { CurrencyCd = "USD", SourceFrom = "LAYER", SourceTo = "LAYER", Active = true });
                        rates.AddRange(CreateCurrencyPairsForSourceListAsync(sourceList, usdOnly, source).Result); //build all the pairs for usd that are in the database and add
                    }

                    Data.APIHandlers.CurrencyLayerAPIHandler clAPI = new Data.APIHandlers.CurrencyLayerAPIHandler();
                    List<Data.Entities.BasicRateEntity> clRates = clAPI.GetUSDCurrencyRates();

                    foreach (Data.Entities.BasicRateEntity clr in clRates)
                    {
                        if (clr.CurrencyFrom != null)
                        {
                            APICurrencyRates ar = rates.SingleOrDefault(rr => rr.CurrencyCdFrom == clr.CurrencyFrom && rr.CurrencyCdTo == clr.CurrencyTo);
                            if (ar != null)
                            {
                                ar.Rate = clr.Rate;
                                ar.TimeUpdated = clr.TimeUpdated;
                            }
                        }
                    }

                    CalculateCrossRates(ref rates, clRates.FirstOrDefault().CurrencyFrom);
                    CalculateOppositePairsAndAddToList(ref rates);

                    // Debug.WriteLine("Done currencylayer");
                }
                else if (source == Sources.Cryptonator)
                {
                 
                    //use btc if in db, else just get the first one and deal
                    string baseRate = (rates.Any(r => r.CurrencyCdFrom == "BTC")) ? "BTC" : rates.FirstOrDefault(r => r.Rate == -1).CurrencyCdFrom;
                   
                     IEnumerable<APICurrencyRates> nextBaseRates = rates.Where(r => r.CurrencyCdFrom == baseRate);

                           // while (.Any()) //while pairs in the queue
                     for(int i = 0; i < nextBaseRates.Count();i++)
                     {
                                APICurrencyRates pair = nextBaseRates.ElementAt(i);//calculateQueue.Dequeue();
                                //call api handler
                                Data.APIHandlers.CryptonatorAPIHandler crAPI = new Data.APIHandlers.CryptonatorAPIHandler();
                                BasicRateEntity clr = crAPI.GetRateForCurrency(pair.CurrencyPairID);
                     //           Debug.WriteLine("rate retrieved");
                                //we got something back
                                if (clr.CurrencyFrom != null)
                                {
                                    //update the rate
                                    APICurrencyRates ar = rates.SingleOrDefault(rr => rr.CurrencyCdFrom == clr.CurrencyFrom && rr.CurrencyCdTo == clr.CurrencyTo);
                                    if (ar != null)
                                    {
                                        ar.Rate = clr.Rate;
                                        ar.TimeUpdated = clr.TimeUpdated;
                                    }

                                }
                     }
                  
                    //then see if we can calculate any other pairs from this call given a pair set using cross rates. This keeps the api calls down, hopefully.
                     CalculateCrossRates(ref rates, baseRate);
                     CalculateOppositePairsAndAddToList(ref rates); //this should also keep calls down, doing this here.
           
                    //Debug.WriteLine("Done crypto");

                }
                else if (source == Sources.Yahoo)
                {
                    //nothing right now
                }
                else
                    throw new NotSupportedException("The source provided is not a supported currency source.");
            }
                    
        }

        private static void CalculateCrossRates(ref List<APICurrencyRates> rates, string baseRate)
        {
            if (rates.Any(ra => ra.CurrencyCdFrom == baseRate && ra.Rate > 0))
            {
                List<APICurrencyRates> otherRatesFound = rates.Where(r2 => r2.CurrencyCdFrom == baseRate && r2.Rate != 0).ToList();
                IEnumerable<APICurrencyRates> ratesToCalc = rates.Where(r3 => otherRatesFound.Any(or => or.CurrencyCdTo == r3.CurrencyCdFrom) && r3.Rate == -1);
                foreach (APICurrencyRates rc in ratesToCalc.ToList())
                {
                    APICurrencyRates rate1 = rates.SingleOrDefault(r11 => r11.CurrencyCdFrom == baseRate && r11.CurrencyCdTo == rc.CurrencyCdTo);
                    APICurrencyRates rate2 = rates.SingleOrDefault(r12 => r12.CurrencyCdFrom == baseRate && r12.CurrencyCdTo == rc.CurrencyCdFrom);
                    if (rate1 != null && rate2 != null && rate1.Rate > 0 && rate2.Rate > 0) //if we somehow found a bad rate, don't use it
                    {
                        rc.Rate = rate1.Rate * (1 / rate2.Rate);
                        rc.TimeUpdated = rate1.TimeUpdated;
                    }
                }
            }
        }

        private static void CalculateOppositePairsAndAddToList(ref List<APICurrencyRates> rates)
        {
            foreach (APICurrencyRates cr in rates.ToList())
            {
                APICurrencyRates ocr;
                if (!rates.Any(r => r.CurrencyCdFrom == cr.CurrencyCdTo && r.CurrencyCdTo == cr.CurrencyCdFrom))
                {
                    ocr = new APICurrencyRates(cr.CurrencyCdTo, cr.CurrencyCdFrom, cr.Delimiter)
                    {
                        Rate = (cr.Rate == 0) ? 0 : (1 / cr.Rate),
                        TimeUpdated = cr.TimeUpdated
                    };
                    rates.Add(ocr);
                }
                else if (rates.Any(r => r.CurrencyCdFrom == cr.CurrencyCdTo && r.CurrencyCdTo == cr.CurrencyCdFrom && r.Rate <= 0))
                {
                    ocr = rates.SingleOrDefault(r => r.CurrencyCdFrom == cr.CurrencyCdTo && r.CurrencyCdTo == cr.CurrencyCdFrom);
                    ocr.Rate = (cr.Rate == 0) ? 0 : (1 / cr.Rate);
                    ocr.TimeUpdated = cr.TimeUpdated;
                }
            }
        }

        private static void GenerateRandomRates(ref List<APICurrencyRates> rates)
        {
            throw new NotImplementedException();
        }

        private static void SaveRates(List<APICurrencyRates> rates)
        {
            //check to make sure usd is in currency table.  If not, take it out (it was added due to base calculations)
            CurrencySourceLists sourceList = new CurrencySourceLists(false);
            if (!sourceList.FullCurrencyList.Any(fc => fc.CurrencyCd == "USD"))
            {
                List<APICurrencyRates> usdRates = rates.Where(r => r.CurrencyCdFrom == "USD").ToList();
                Parallel.ForEach(usdRates, ur =>
                {
                    rates.Remove(ur);
                });
            }

            //save it
            
            IEnumerable<APICurrencyRates> ratesToSave = rates.Where(r => r.Rate > 0);
            int rCount = ratesToSave.Count();//rates.Count();
            if (ratesToSave.Any())
            {

                if (ratesToSave.Count() > 5000)
                {
                    int position = 0;
                    int take = 5000;
                    while (position < rCount)
                    {
                        if (rCount - (position + take) < 0)
                        {
                            take = (rCount - position);
                        }
                        // Debug.WriteLine(position);
                        CurrencyRatesTableTypeCollection ttParam = new CurrencyRatesTableTypeCollection();
                        ttParam.AddRange(ratesToSave.Skip(position).Take(take));
                        position = position + take;

                        if (ttParam.Count > 0)
                        {
                            SaveRates(ttParam);
                        }
                        else
                        {
                            break; //if we got nothing on skip, take, we're at the end...
                        }
                    }

                }
                else
                {
                    CurrencyRatesTableTypeCollection ttParam = new CurrencyRatesTableTypeCollection();
                    ttParam.AddRange(ratesToSave);
                    SaveRates(ttParam);
                }
            }
            

        }

        private static void SaveRates(CurrencyRatesTableTypeCollection ttParam)
        {
            try
            {
                using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
                {
                    SqlParameter[] sparams = new SqlParameter[1];
                    sparams[0] = new SqlParameter("CurrencyRateList", SqlDbType.Structured);
                    sparams[0].Direction = System.Data.ParameterDirection.Input;
                    sparams[0].Value = ttParam;

                    //Debug.WriteLine("About to save rates");
                    context.Database.CommandTimeout = 900000000;
                    context.ExecuteNonResultSproc("usp_SaveCurrencyRates", sparams);

                }
            }
            catch(Exception ex)
            {
                string errorLogPath = @"C:\UpdateRateBatch\ErrorLog.txt";
                File.AppendAllText(errorLogPath, Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
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
