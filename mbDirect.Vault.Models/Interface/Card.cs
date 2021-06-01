using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mbDirect.Vault.Models.Data;

namespace mbDirect.Vault.Models.Interface
{
    public class Card
    {
        #region Constructor
        /// <summary>
        /// Constructor, standard
        /// </summary>
        public Card() { }
        /// <summary>
        /// Constructor, use Account to preset values
        /// </summary>
        /// <param name="acct"></param>
        public Card(Account acct)
        {
            FromAccount(acct);
        }
        #endregion

        public String InstrumentTypeCode { get; set; }
        public DateTime AddDateTime { get; set; }
        public String OwnerName { get; set; }
        public String MaskedNumber { get; set; }
        public String RoutingNumber { get; set; }
        public String Expiration { get; set; }
        public String BillingZipCode { get; set; }
        public String AccountStatusCode { get; set; }
        public DateTime StatusDateTime { get; set; }

        public void FromAccount(Account acct)
        {
            if (acct == null)
                return;

            InstrumentTypeCode = acct.InstrumentTypeCode;
            AddDateTime = acct.AddDateTime;
            OwnerName = acct.OwnerName;

            if (!String.IsNullOrEmpty(acct.Number))
            {
                String filler = "xxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                if (acct.Number.Length >= 9)
                {
                    MaskedNumber = String.Format("{0}-{1}-{2}",
                                            acct.Number.Substring(0, 4),
                                            acct.Number.Length > 4 ? filler.Substring(0, 4) + "-" + filler.Substring(0, acct.Number.Length - 12) : filler.Substring(0, 4), 
                                            acct.Number.Substring(acct.Number.Length - 4, 4));
                }
            }
            Expiration = acct.Expiration;
            BillingZipCode = acct.BillingZipCode;
            AccountStatusCode = acct.AccountStatusCode;
            StatusDateTime = acct.StatusDateTime;
        }
    }
}
