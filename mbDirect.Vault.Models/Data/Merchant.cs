using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.Data
{
    public class Merchant
    {
        public String MerchantId { get; set; }
        public String DeviceId { get; set; }
        public TransitCredential TransitUser { get; set; }
        public String TransitTransactionKey { get; set; }
        public DateTime LastMaintDate { get; set; }
    }
}
