﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.BusinessLibrary.ViewModels;
using Ninject;
using CuMaster.BusinessLibrary.Classes.Session;
using System.Web;
using CuMaster.Security;

namespace CuMaster.BusinessLibrary.Library
{
    public class EmailAlertLibrary
    {
        public CurrencyLibrary Currencies { get; set; }
        private string SessionID { get; set; }
        private string DefaultCurrencyFrom { get; set; }
        private string DefaultCurrencyTo { get; set; }

        public EmailAlertLibrary(HttpContext context, Session session)
        {
            this.SessionID = (context.User.Identity.IsAuthenticated) ? context.User.Identity.Name : session.SessionID;
            this.DefaultCurrencyFrom = session.Defaults.DefaultCurrencyFrom;
            this.DefaultCurrencyTo = session.Defaults.DefaultCurrencyTo;
        }

        public ViewModels.EmailAlertViewModel GetInitalSettings()
        {
            this.Currencies = new Library.CurrencyLibrary();

            Models.EmailAlertModel newAlert = new Models.EmailAlertModel();

            //get user settings
            newAlert.CurrencyFrom = this.DefaultCurrencyFrom;
            newAlert.CurrencyTo = this.DefaultCurrencyTo;

            newAlert.PercentageChange = 0;
            newAlert.TimeToSend = DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0));

            return GetEmailAlert(newAlert);
        }

       

        public void SaveEmailAlert(EmailAlertViewModel emailAlert)
        {
            var eaRes = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IEmailAlertRepository>();
            eaRes.Save(new Data.Entities.EmailAlertEntity
            {
                CurrencyFrom = emailAlert.CurrencyFrom,
                CurrencyTo = emailAlert.CurrencyTo,
                Email = emailAlert.Email,
                SessionID = this.SessionID
            });
        }

        public void DeleteAllAlerts(string email)
        {
            var eaRes = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IEmailAlertRepository>();
            eaRes.DeleteAllForEmail(email);
        }

        public ViewModels.EmailAlertViewModel GetEmailAlert(Models.EmailAlertModel eam)
        {
            return new ViewModels.EmailAlertViewModel
            {
                Currencies = this.Currencies.CurrencyFromSelect,
                CurrenciesTo = this.Currencies.GetAllowedCurrenciesSelect(eam.CurrencyFrom),
                Email = eam.Email,
                CurrencyFrom = eam.CurrencyFrom,
                CurrencyTo = eam.CurrencyTo,
                PercentageChange = eam.PercentageChange,
                TimeToSend = eam.TimeToSend
            };
        }

        public ViewModels.EmailAlertViewModel GetEmailAlert(EmailAlertViewModel emailAlert)
        {
            EmailAlertViewModel updatedAlert = emailAlert;
            updatedAlert.CurrenciesTo = this.Currencies.GetAllowedCurrenciesSelect(emailAlert.CurrencyFrom);
            return updatedAlert;
        }
    }
}
