using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class EmailAlertRecordViewModel
    {
        public int AlertID { get; set; }

        public string Email { get; set; }

        public string CurrencyFrom { get; set; }

        public string CurrencyTo { get; set; }

        public string CurrencyFromName { get; set; }

        public string CurrencyToName { get; set; }

        public decimal? PercentageChange { get; set; }

        public TimeSpan? TimeToSend { get; set; }
        public string TimeToSendUTCStr
        {
            get
            {
                if (this.TimeToSend == null)
                    return null;

                TimeSpan time = this.TimeToSend ?? DateTime.Now.TimeOfDay;
                return new DateTime(time.Ticks).AddDays(1).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }
        }
        
        public DateTime DateCreated { get; set; }
        public string DateCreatedUTCStr
        {
            get
            {
                return this.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }
        }
        public DateTime? LastSent { get; set;  }

        public string LastSentUTCStr
        {
            get
            {
                if (this.LastSent == null)
                    return null;

                DateTime sent = this.LastSent ?? DateTime.Now;
                return sent.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }
        }
    }
}
