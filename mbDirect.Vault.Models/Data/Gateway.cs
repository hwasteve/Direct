using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.Data
{
    public class Gateway
    {
        public int Id { get; set; }
        public String Description { get; set; }
        public String EndPoint { get; set; }
        public DateTime LastMaintDate { get; set; }
    }
}
