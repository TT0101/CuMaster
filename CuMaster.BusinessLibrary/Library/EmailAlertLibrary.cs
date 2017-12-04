using System;
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
        private string SessionID { get; set; }
        private string DefaultCurrencyFrom { get; set; }
        private string DefaultCurrencyTo { get; set; }
        private string DefaultEmail { get; set; }

        public EmailAlertLibrary(HttpContext context, Session session)
        {
            this.SessionID = (context.User.Identity.IsAuthenticated) ? context.User.Identity.Name : session.SessionID;
            this.DefaultCurrencyFrom = session.Defaults.DefaultCurrencyFrom;
            this.DefaultCurrencyTo = session.Defaults.DefaultCurrencyTo;
            this.DefaultEmail = session.Defaults.Email;
        }

        public ViewModels.EmailAlertViewModel GetInitalSettings()
        {
            Models.EmailAlertModel newAlert = new Models.EmailAlertModel();

            //get user settings
            newAlert.Email = this.DefaultEmail;
            newAlert.CurrencyFrom = this.DefaultCurrencyFrom;
            newAlert.CurrencyTo = this.DefaultCurrencyTo;

            newAlert.PercentageChange = 0;
            newAlert.TimeToSend = DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0));

            return GetEmailAlert(newAlert);
        }

        public ViewModels.EmailAlertViewModel GetForExistingAlert(int alertID)
        {
            var eaRes = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IEmailAlertRepository>();
            Data.Entities.EmailAlertEntity alert = eaRes.GetAlert(alertID);
            return GetEmailAlert(new Models.EmailAlertModel
            {
                Email = alert.Email,
                CurrencyFrom = alert.CurrencyFrom,
                CurrencyTo = alert.CurrencyTo,
                PercentageChange = alert.Threshold,
                TimeToSend = alert.TimeToSend,
            }, true);
        }

        public void SaveEmailAlert(EmailAlertViewModel emailAlert)
        {
            var eaRes = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IEmailAlertRepository>();
            eaRes.Save(new Data.Entities.EmailAlertEntity
            {
                CurrencyFrom = emailAlert.CurrencyFrom,
                CurrencyTo = emailAlert.CurrencyTo,
                Email = emailAlert.Email,
                Threshold = emailAlert.PercentageChange,
                TimeToSend = emailAlert.TimeToSend,
                SessionID = this.SessionID
            });
        }

        public void DeleteAllAlerts(string email)
        {
            var eaRes = DIResolver.Data.NinjectConfig.GetKernal().Get<Data.RepositoryInterfaces.IEmailAlertRepository>();
            eaRes.DeleteAllForEmail(email);
        }

        public ViewModels.EmailAlertViewModel GetEmailAlert(Models.EmailAlertModel eam, bool isForEdit = false)
        {
            CurrencyLibrary cl = new CurrencyLibrary();
            return new ViewModels.EmailAlertViewModel
            {
                Currencies = cl.CurrencyFromSelect,
                CurrenciesTo = cl.GetAllowedCurrenciesSelect(eam.CurrencyFrom),
                Email = eam.Email,
                CurrencyFrom = eam.CurrencyFrom,
                CurrencyTo = eam.CurrencyTo,
                PercentageChange = eam.PercentageChange,
                TimeToSend = eam.TimeToSend,
                IsForEdit = isForEdit
            };
        }

        public ViewModels.EmailAlertViewModel GetEmailAlert(EmailAlertViewModel emailAlert)
        {
            CurrencyLibrary cl = new CurrencyLibrary();
            EmailAlertViewModel updatedAlert = emailAlert;
            updatedAlert.CurrenciesTo = cl.GetAllowedCurrenciesSelect(emailAlert.CurrencyFrom);
            return updatedAlert;
        }
    }
}
