using System;
using System.Collections.Generic;
using System.Text;

namespace CuMaster.BusinessLibrary.Models
{
    public class CountryModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public CountryModel(Data.Entities.CountryEntity country)
        {
            this.Code = country.Code;
            this.Name = country.Name;
        }
    }
}
