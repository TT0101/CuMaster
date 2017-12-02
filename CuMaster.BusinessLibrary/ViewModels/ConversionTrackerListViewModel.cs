using HelperFramework.UI.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class ConversionTrackerListViewModel
    {
        //public List<ConversionTrackerViewModel> EntryList { get; set; }
        public DataTableObject<IEnumerable<ViewModels.ConversionTrackerViewModel>> EntryListDataTable { get; set; }

        public ConversionTrackerListViewModel(DataTableObject<IEnumerable<Data.Entities.ConversionTrackerEntity>> dt) //needs to take in the data to create the datatableobject properly, from library?
        {
            this.EntryListDataTable = new DataTableObject<IEnumerable<ConversionTrackerViewModel>>
            {
                data = ((IEnumerable<Data.Entities.ConversionTrackerEntity>)dt.data).Select(t => new ConversionTrackerViewModel
                {
                    AmountFrom = t.FromAmount,
                    AmountTo = t.ToAmount,
                    EntryID = t.EntryID,
                    EntryName = t.EntryName,
                    AutoUpdate = t.UpdateRate,
                    CurrencyFrom = t.CurrencyFrom,
                    CurrencyTo = t.CurrencyTo,
                    LastUpdated = t.LastUpdatedDate,
                    RateUsed = t.RateToUse,
                    IsCrypto = t.IsCrypto
                }),
                extraData = dt.extraData,
                recordsFiltered = dt.recordsFiltered,
                recordsTotal = dt.recordsTotal
            };
        }

        //public void AddEntry(ConversionTrackerViewModel entry) //test code for now.  get from db, don't have to worry about this
        //{
        //    this.EntryList.Add(entry);
        //    this.EntryListDataTable.data = this.EntryList;
        //    this.EntryListDataTable.recordsTotal = this.EntryList.Count;
        //}
    }
}
