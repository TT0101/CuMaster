using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        private string _pw;
        public string Password
        {
            get
            {
                return _pw;
            }
            set
            {
                _pw = Security.PasswordHash.HashPassword(value);
            }
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

       
    }
}
