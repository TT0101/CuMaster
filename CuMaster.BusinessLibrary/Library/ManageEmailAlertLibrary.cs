using HelperFramework.UI.DataTables;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.Library
{
    public class ManageEmailAlertLibrary
    {
        public ManageEmailAlertLibrary()
        {

        }

        public ViewModels.EmailAlertListViewModel GetUserEmailAlerts(string userName, DataTableParams dtParams)
        {
            var ctRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IEmailAlertRepository>();
            ViewModels.EmailAlertListViewModel ctvm = new ViewModels.EmailAlertListViewModel(ctRes.GetForDataTable(userName, dtParams));

            return ctvm;
        }

        public void DeleteEmailAlert(int alertID)
        {
            var ctRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IEmailAlertRepository>();
            ctRes.Delete(alertID);
        }

        public void DeleteAllUserAlerts(string userName)
        {
            var ctRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IEmailAlertRepository>();
            ctRes.DeleteAllForUser(userName);
        }
    }
}
