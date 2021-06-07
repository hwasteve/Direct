using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.Interface
{
    public class TransitCredentialRequest : Data.TransitCredential
    {
        private new String PasswordEncrypted { get; set; }

        public String Password { get; set; }
    }
}
