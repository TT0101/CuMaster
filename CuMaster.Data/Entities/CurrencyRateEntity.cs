﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CuMaster.Data.Entities
{
    public class CurrencyRateEntity
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}
