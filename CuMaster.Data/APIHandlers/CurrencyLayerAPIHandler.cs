using CuMaster.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using CuMaster.Data.APIObjects;

namespace CuMaster.Data.APIHandlers
{
    public class CurrencyLayerAPIHandler
    {
        private static string url = "http://www.apilayer.net/api/live?access_key=[keygoeshere]";

        public List<BasicRateEntity> GetUSDCurrencyRates()
        {
            CurrencyLayerResponse currencyList;
            List<BasicRateEntity> rateList = new List<BasicRateEntity>();
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var jsonStr = wc.DownloadString(url);
                    currencyList = JsonConvert.DeserializeObject<CurrencyLayerResponse>(jsonStr);
                }

                
                if (currencyList.success)
                {
                    foreach(KeyValuePair<string, string> kv in currencyList.quotes)
                    {
                        BasicRateEntity bre = new BasicRateEntity
                        {
                            CurrencyFrom = kv.Key.Substring(0, 3),
                            CurrencyTo = kv.Key.Substring(3),
                            TimeUpdated = DateTimeOffset.FromUnixTimeSeconds(currencyList.timestamp).DateTime
                        };

                        decimal rateTry;
                        if (Decimal.TryParse(kv.Value, out rateTry))
                            bre.Rate = rateTry;
                        else
                            bre.Rate = 0;

                        
                        rateList.Add(bre);
                    }
                }
                else
                {
                    //log error
                     string errorLogPath = @"C:\UpdateRateBatch\ErrorLog.txt";
                     File.AppendAllText(errorLogPath, Environment.NewLine + currencyList.error.code + currencyList.error.info);
                }
            }
            catch(Exception ex)
            {
                //log
                string errorLogPath = @"C:\UpdateRateBatch\ErrorLog.txt";
                File.AppendAllText(errorLogPath, Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return rateList;
        }
    }
}
