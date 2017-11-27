using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.UpdateCurrencyRates
{
    class Program
    {
        static void Main(string[] args)
        {
            RunSchedule();
        }

        public static void RunSchedule()
        {
            string path = Path.GetFullPath("C:\\UpdateRateBatch") + "\\" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss_fff") + "_Log.txt";
            try
            {
                //Console.WriteLine("Start time: " + DateTime.Now.ToString("MM/dd/yyy HH:mm"));
                //if(!File.Exists(path))
                //{
                //    File.Create(path);
                //}
                //File.AppendAllText(path, Environment.NewLine + "Start time: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
                CallLoad();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("ERROR: " + ex.Message + " | " + ex.StackTrace);
                string errorLogPath = @"C:\UpdateRateBatch\ErrorLog.txt";
                File.AppendAllText(errorLogPath, Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            //File.AppendAllText(path, ", End time: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
            //Console.WriteLine("End time?: " + DateTime.Now.ToString("MM/dd/yyy HH:mm"));
            //Console.WriteLine("Hit any key to exit...");
            //var x = Console.ReadLine();

        }

        public static void CallLoad()
        {
           CurrencyRateRetrieval.LoadCurrencyRates();
        }
    }
}
