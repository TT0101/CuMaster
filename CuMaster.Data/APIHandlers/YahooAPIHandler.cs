using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CuMaster.Data.APIHandlers
{
    public class YahooAPIHandler
    {
        //code is from https://stackoverflow.com/questions/45857162/no-definition-found-for-table-yahoo-finance-xchange?noredirect=1&lq=1 
        //private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private int timeout = 4000;
        private int Try { get; set; }

        public decimal GetRate(string from, string to)
        {
            var url =
                string.Format(
                    "http://finance.yahoo.com/d/quotes.csv?e=.csv&f=sl1d1t1&s={0}{1}=X", from, to);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.UseDefaultCredentials = true;
            request.ContentType = "text/csv";
            request.Timeout = timeout;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var resStream = response.GetResponseStream();
                    using (var reader = new StreamReader(resStream))
                    {
                        var html = reader.ReadToEnd();
                        var values = Regex.Split(html, ",");
                        var rate = Convert.ToDecimal(values[1], new CultureInfo("en-US"));
                        if (rate == 0)
                        {
                            Thread.Sleep(550);
                            ++Try;
                            return Try < 5 ? GetRate(from, to) : 0;
                        }
                        return rate;

                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Warning("Get currency rate from Yahoo fail " + ex);
                Thread.Sleep(550);
                ++Try;
                return Try < 5 ? GetRate(from, to) : 0;
            }
        }
    }
}
