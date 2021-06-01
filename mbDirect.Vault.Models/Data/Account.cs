using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.Data
{
    public class Account
    {
        public long Id { get; set; }
        public String InstrumentTypeCode { get; set; }
        public DateTime AddDateTime { get; set; }
        public String OwnerName { get; set; }
        public String Number { get; set; }
        public String RoutingNumber { get; set; }
        public String Expiration { get; set; }
        public String BillingZipCode { get; set; }
        public String AccountStatusCode { get; set; }
        public DateTime StatusDateTime { get; set; }
    }
}
