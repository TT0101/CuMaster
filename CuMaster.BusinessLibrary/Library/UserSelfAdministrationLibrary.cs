using CuMaster.BusinessLibrary.Classes.Session;
using CuMaster.BusinessLibrary.Lookups;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CuMaster.BusinessLibrary.Library
{
    public class UserSelfAdministrationLibrary
    {
        private string UserID { get; set; }
        private Session session { get; set; }
        private string Email { get; set; }

        public UserSelfAdministrationLibrary(HttpContext context, Session session)
        {
            this.UserID = session.UserID;
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IUserRepository>();
            Data.Entities.UserEntity user = res.GetUser(this.UserID);
            this.Email = user.Email;
        }

        public ViewModels.UserDashboardViewModel GetUserDashboardInformation()
        {
            CurrencyLibrary cLib = new CurrencyLibrary();
            CountryLookup cLook = new CountryLookup();
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IUserRepository>();
            Data.Entities.UserEntity user = res.GetUser(this.UserID);

            return new ViewModels.UserDashboardViewModel
            {
                UserName = this.UserID,
                AutoUpdateTrackerDefault = user.AutoUpdateEntries,
                DefaultCountry = (user.DefaultCountry) ?? "",
                DefaultCurrencyFrom = user.DefaultCurrencyFrom,
                DefaultCurrencyTo = (user.DefaultCurrencyTo) ?? "",
                DisplayName = user.DisplayName,
                Email = user.Email,
                Countries = cLook.GetSelectList(),
                Currencies = cLib.CurrencyFromSelect,
                CurrenciesTo = cLib.GetAllowedCurrenciesSelect((user.DefaultCurrencyFrom) ?? "")
            };
        }

        public ViewModels.UserDashboardViewModel GetUserDashboardFromViewModel(ViewModels.UserDashboardViewModel udvm)
        {
            CurrencyLibrary cLib = new CurrencyLibrary();
            CountryLookup cLook = new CountryLookup();

            return new ViewModels.UserDashboardViewModel
            {
                UserName = this.UserID,
                AutoUpdateTrackerDefault = udvm.AutoUpdateTrackerDefault,
                DefaultCountry = udvm.DefaultCountry,
                DefaultCurrencyFrom = udvm.DefaultCurrencyFrom,
                DefaultCurrencyTo = udvm.DefaultCurrencyTo,
                DisplayName = udvm.DisplayName,
                Email = udvm.Email,
                Countries = cLook.GetSelectList(),
                Currencies = cLib.CurrencyFromSelect,
                CurrenciesTo = cLib.GetAllowedCurrenciesSelect(udvm.DefaultCurrencyFrom)
            };
        }

        public string GetCurrentSavedEmail()
        {
            return this.Email;
        }

        public void SaveUserProfile(ViewModels.UserDashboardViewModel userview)
        {
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IUserRepository>();
            res.UpdateProfile(new Data.Entities.UserEntity
            {
                UserName = userview.UserName,
                Email = userview.Email,
                DisplayName = userview.DisplayName
            });
        }

        public void SaveUserDefaults(ViewModels.UserDashboardViewModel userview)
        {
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IUserRepository>();
            res.UpdateDefaults(new Data.Entities.UserEntity
            {
                UserName = userview.UserName,
                AutoUpdateEntries = userview.AutoUpdateTrackerDefault,
                DefaultCountry = userview.DefaultCountry,
                DefaultCurrencyFrom = userview.DefaultCurrencyFrom,
                DefaultCurrencyTo = userview.DefaultCurrencyTo
            });
        }
    }
}
