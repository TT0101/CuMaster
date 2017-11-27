using HelperFramework.UI.DataTables;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.BusinessLibrary.ViewModels;
using CuMaster.Data.Entities;
using CuMaster.BusinessLibrary.Models;

namespace CuMaster.BusinessLibrary.Library
{
    public class CurrencyConversionTrackerLibrary
    {
        private string SessionID { get; set; }
        private bool DefaultAutoUpdate { get; set; }
        public CurrencyConversionTrackerLibrary()
        {
            this.SessionID = "FFFFFF"; //get this from session object later
            this.DefaultAutoUpdate = true; //this comes from user defaults later
        }

        public ViewModels.ConversionTrackerListViewModel GetTrackerListForUser(DataTableParams dtParams)
        {
            var ctRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IConversionTrackerRepository>();
            ViewModels.ConversionTrackerListViewModel ctvm = new ViewModels.ConversionTrackerListViewModel(ctRes.GetForDataTable(this.SessionID, dtParams));

            return ctvm;
        }

        public void SaveNewEntry(ConversionTrackerViewModel tvm)
        {
            
            ConversionTrackerEntity cte = new ConversionTrackerEntity
            {
                EntryName = tvm.EntryName,
                FromAmount = tvm.AmountFrom,
                ToAmount = tvm.AmountTo,
                SessionID = this.SessionID,
                UpdateRate = this.DefaultAutoUpdate,
                CurrencyFrom = tvm.CurrencyFrom,
                CurrencyTo = tvm.CurrencyTo,
                RateToUse = tvm.RateUsed,
                LastUpdatedDate = tvm.LastUpdated
            };
            var ctRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IConversionTrackerRepository>();
            ctRes.Save(cte);
        }

        public void SaveAutoUpdateChange(int entryID)
        {
            var ctRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IConversionTrackerRepository>();
            ctRes.SaveExisting(entryID);
        }

        public void DeleteEntry(int entryID)
        {
            var ctRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IConversionTrackerRepository>();
            ctRes.Delete(entryID);
        }

        public void DeleteAllEntriesForUser()
        {
            var ctRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IConversionTrackerRepository>();
            ctRes.DeleteAll(this.SessionID);
        }
    }
}
