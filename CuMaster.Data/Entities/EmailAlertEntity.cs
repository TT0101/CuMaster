using System;

namespace CuMaster.Data.Entities
{
    public class EmailAlertEntity
    {
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public string Email { get; set; }
        public decimal Threshold { get; set; }
        public TimeSpan TimeToSend { get; set; }
        public string SessionID { get; set; }
        public int AlertID { get; set; }
        public DateTime LastSent { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal InitalRate { get; set; }

        public EmailAlertEntity()
        {

        }
    }
}