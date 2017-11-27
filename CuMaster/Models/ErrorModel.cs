using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuMaster.Models
{
    public class ErrorModel
    {
        public string ErrorMessage { get; set; }
        private Exception InnerException { get; set; }

        public ErrorModel()
        {
            this.ErrorMessage = String.Empty;
        }

        public ErrorModel(Exception ex)
        {
            this.ErrorMessage = this.GetAllMessages(ex);
        }

        private string GetAllMessages(Exception ex)
        {
            if(ex.InnerException != null)
            {
                return ex.Message + " | " + this.GetAllMessages(ex.InnerException);
            }

            return ex.Message;
        }
    }
}