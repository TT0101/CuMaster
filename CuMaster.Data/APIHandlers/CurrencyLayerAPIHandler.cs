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
        private static string url = "http://www.apilayer.net/api/live?access_key=4446058fea35802b02872d4626979443";

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

                //string test = "{'success':true,'timestamp':1511051351,'source':'USD','quotes':{'USDAED':3.672804,'USDAFN':68.559998,'USDALL':112.949997,'USDAMD':486.279999,'USDANG':1.780403,'USDAOA':165.098007,'USDARS':17.459041,'USDAUD':1.319904,'USDAWG':1.78,'USDAZN':1.699704,'USDBAM':1.661104,'USDBBD':2,'USDBDT':83.440002,'USDBGN':1.658204,'USDBHD':0.377404,'USDBIF':1742.97998,'USDBMD':1,'USDBND':1.353204,'USDBOB':6.860399,'USDBRL':3.257104,'USDBSD':1,'USDBTC':0.000129,'USDBTN':65.175003,'USDBWP':10.510204,'USDBYN':1.970398,'USDBYR':19600,'USDBZD':1.997704,'USDCAD':1.275904,'USDCDF':1565.50392,'USDCHF':0.988041,'USDCLF':0.02325,'USDCLP':625.919983,'USDCNY':6.624404,'USDCOP':2996.300049,'USDCRC':563.750395,'USDCUC':1,'USDCUP':26.5,'USDCVE':93.503897,'USDCZK':21.674999,'USDDJF':177.199997,'USDDKK':6.31041,'USDDOP':47.220001,'USDDZD':114.450996,'USDEGP':17.603881,'USDERN':15.280392,'USDETB':27.000358,'USDEUR':0.847904,'USDFJD':2.08904,'USDFKP':0.756104,'USDGBP':0.75677,'USDGEL':2.693904,'USDGGP':0.756759,'USDGHS':4.563504,'USDGIP':0.756404,'USDGMD':47.049999,'USDGNF':8998.000355,'USDGTQ':7.34204,'USDGYD':206.490005,'USDHKD':7.809204,'USDHNL':23.459999,'USDHRK':6.411104,'USDHTG':61.700001,'USDHUF':264.160004,'USDIDR':13523,'USDILS':3.509804,'USDIMP':0.756759,'USDINR':65.009003,'USDIQD':1167,'USDIRR':35237.000352,'USDISK':103.099998,'USDJEP':0.756759,'USDJMD':125.180386,'USDJOD':0.706804,'USDJPY':111.996002,'USDKES':103.449997,'USDKGS':69.72204,'USDKHR':4022.300049,'USDKMF':423.399994,'USDKPW':900.00035,'USDKRW':1092.819946,'USDKWD':0.301604,'USDKYD':0.820383,'USDKZT':331.73999,'USDLAK':8297.000349,'USDLBP':1505.503779,'USDLKR':153.550003,'USDLRD':124.199997,'USDLSL':13.980382,'USDLTL':3.048704,'USDLVL':0.62055,'USDLYD':1.366304,'USDMAD':9.419039,'USDMDL':17.495001,'USDMGA':3160.000347,'USDMKD':51.98038,'USDMMK':1363.000346,'USDMNT':2440.000346,'USDMOP':8.045804,'USDMRO':351.000346,'USDMUR':33.900002,'USDMVR':15.570378,'USDMWK':716.400024,'USDMXN':18.910378,'USDMYR':4.160378,'USDMZN':60.220001,'USDNAD':13.966039,'USDNGN':358.000344,'USDNIO':30.403725,'USDNOK':8.233204,'USDNPR':103.550003,'USDNZD':1.466404,'USDOMR':0.384504,'USDPAB':1,'USDPEN':3.236804,'USDPGK':3.241504,'USDPHP':50.799999,'USDPKR':105.129997,'USDPLN':3.588504,'USDPYG':5641.000341,'USDQAR':3.838504,'USDRON':3.930704,'USDRSD':100.027496,'USDRUB':59.081001,'USDRWF':832.289978,'USDSAR':3.749904,'USDSBD':7.840604,'USDSCR':13.421038,'USDSDG':6.659704,'USDSEK':8.439038,'USDSGD':1.35491,'USDSHP':0.756404,'USDSLL':7620.000339,'USDSOS':557.000338,'USDSRD':7.380371,'USDSTD':20775,'USDSVC':8.75037,'USDSYP':514.97998,'USDSZL':13.957038,'USDTHB':32.810001,'USDTJS':8.811904,'USDTMT':3.4,'USDTND':2.495504,'USDTOP':2.282204,'USDTRY':3.875504,'USDTTD':6.707504,'USDTWD':30.048038,'USDTZS':2234.000336,'USDUAH':26.465038,'USDUGX':3625.000335,'USDUSD':1,'USDUYU':29.370001,'USDUZS':8055.000335,'USDVEF':9.975038,'USDVND':22699,'USDVUV':108.000334,'USDWST':2.562904,'USDXAF':555.840027,'USDXAG':0.057762,'USDXAU':0.000773,'USDXCD':2.703606,'USDXDR':0.708747,'USDXOF':555.429993,'USDXPF':101.625037,'USDYER':249.800003,'USDZAR':13.969804,'USDZMK':9001.203593,'USDZMW':10.010363,'USDZWL':322.355011}}";

                //currencyList = JsonConvert.DeserializeObject<CurrencyLayerResponse>(test);


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
                }
            }
            catch(Exception ex)
            {
                //log
            }

            return rateList;
        }
    }
}
