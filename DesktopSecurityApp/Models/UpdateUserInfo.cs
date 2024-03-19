using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSecurityApp.Models
{
    public class UpdateUserInfo
    {
        public string NewUsername { get; set; }
        public string NewKey { get; set; }
        public string NewEmail { get; set; }

         public override string ToString()
        {
            return $"New Username: {NewUsername}, New Key: {NewKey}, New Email: {NewEmail}";
        }
    }
}
