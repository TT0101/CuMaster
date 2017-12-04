using HelperFramework.UI.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class EmailAlertListViewModel
    {
        public DataTableObject<IEnumerable<ViewModels.EmailAlertRecordViewModel>> ListDataTable { get; set; }

        public EmailAlertListViewModel(DataTableObject<IEnumerable<Data.Entities.EmailAlertRecordEntity>> dt) //needs to take in the data to create the datatableobject properly, from library?
        {
            this.ListDataTable = new DataTableObject<IEnumerable<EmailAlertRecordViewModel>>
            {
                data = ((IEnumerable<Data.Entities.EmailAlertRecordEntity>)dt.data).Select(t => new EmailAlertRecordViewModel
                {
                    AlertID = t.AlertID,
                    CurrencyFrom = t.CurrencyFrom,
                    CurrencyFromName = t.CurrencyFromName,
                    CurrencyTo = t.CurrencyTo,
                    CurrencyToName = t.CurrencyToName,
                    DateCreated = t.DateCreated,
                    Email = t.Email,
                    LastSent = t.LastSent,
                    PercentageChange = t.PercentageChange,
                    TimeToSend = t.TimeToSend
                }),
                extraData = dt.extraData,
                recordsFiltered = dt.recordsFiltered,
                recordsTotal = dt.recordsTotal
            };
        }
    }
}
