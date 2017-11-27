using CuMaster.Data.APIObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.APIHandlers
{
    public class CryptonatorAPIHandler
    {
        private static string url = "https://api.cryptonator.com/api/ticker/";

        public Entities.BasicRateEntity GetRateForCurrency(string currencyPair)
        {
            CryptonatorResponse currency;
            string urlReq = url + currencyPair;
            Entities.BasicRateEntity bre = new Entities.BasicRateEntity();
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var jsonStr = wc.DownloadString(urlReq);

                    //Debug.WriteLine(urlReq.ToString() + ": " + jsonStr);

                    currency = JsonConvert.DeserializeObject<CryptonatorResponse>(jsonStr);
                }

                
                if (currency.success)
                {
                    bre = new Entities.BasicRateEntity
                    {
                        CurrencyFrom = currency.Ticker.baseCurrency,
                        CurrencyTo = currency.Ticker.target,
                        TimeUpdated = DateTimeOffset.FromUnixTimeSeconds(currency.timestamp).DateTime,
                        Rate = currency.Ticker.price
                    };
                }
                else
                {
                    //log error
                    //Debug.WriteLine("Call not successful, error is: " + currency.error + ", URL is: " + urlReq);

                    //set up a basic entities with 0 as the indicator -- didn't find it
                    bre = new Entities.BasicRateEntity
                    {
                        CurrencyFrom = currencyPair.Split('-')[0],
                        CurrencyTo = currencyPair.Split('-')[1],
                        Rate = 0,
                        TimeUpdated = DateTime.Now
                    };
                        
                }
            }
            catch (Exception ex)
            {
                //log
                Debug.WriteLine("Error: " + ex.Message + ", URL: " + urlReq);
            }

            return bre;
        }
    }
}
