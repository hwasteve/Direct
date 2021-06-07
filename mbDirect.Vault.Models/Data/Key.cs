using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.Data
{
    public class Key
    {
        public int KeyId {get; set;}
        public String KeyValue { get; set; }
        public String Vector { get; set; }
        public DateTime LastMaintDate { get; set; }
    }
}
