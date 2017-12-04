using System;

namespace CuMaster.Data.Entities
{
    public class EmailAlertEntity
    {
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public string Email { get; set; }
        public decimal? Threshold { get; set; }

        private TimeSpan? _time;
        public TimeSpan? TimeToSend
        {
            get
            {
                if(this.TimeToSendDate.TimeOfDay.TotalMinutes > 0 && _time == null)
                {
                    return this.TimeToSendDate.TimeOfDay;
                }
                return _time;
            }
            set
            {
                _time = value;
            }
        }
        public DateTime TimeToSendDate { get; set; }
        public string SessionID { get; set; }
        public int AlertID { get; set; }
        public DateTime? LastSent { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal InitalRate { get; set; }

        public EmailAlertEntity()
        {

        }
    }
}