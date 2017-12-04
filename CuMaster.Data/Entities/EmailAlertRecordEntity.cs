using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.Entities
{
    public class EmailAlertRecordEntity
    {
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public string Email { get; set; }
        public decimal? PercentageChange { get; set; }
        public TimeSpan? TimeToSend { get; set; }
        public string SessionID { get; set; }
        public int AlertID { get; set; }
        public DateTime? LastSent { get; set; }
        public DateTime DateCreated { get; set; }
        public string CurrencyFromName { get; set; }
        public string CurrencyToName { get; set; }
    }
}
