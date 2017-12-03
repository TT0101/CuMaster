using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CuMaster.Security
{
    public class PasswordRules
    {
        public static bool MeetsLengthRule(string password)
        {
            if (password != null && password.Length > 8)
                return true;
            return false;
        }

        public static bool MeetsEntropyRule(string password)
        {
            Regex specalChars = new Regex("[^a-z0-9]");
            if (specalChars.IsMatch(password) && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit))
                return true;

            return false;
        }

        public static bool MeetsNotSameAsUsernameRule(string password, string userName)
        {
            if (password.ToUpper() != userName.ToUpper())
                return true;

            return false;
        }
    }
}
