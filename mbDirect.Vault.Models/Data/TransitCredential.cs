using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.Data
{
    public class TransitCredential
    {
        public int Number { get; set; }
        public String UserId { get; set; }
        public String PasswordEncrypted { get; set; }
        public int PasswordKeyId { get; set; }
        public String DeveloperId { get; set; }
        public Gateway TransitGateway { get; set; }
        public DateTime LastMaintDate { get; set; }
    }
}
