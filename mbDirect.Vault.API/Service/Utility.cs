using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mbDirect.Vault.API.Service
{
    public class Utility
    {
        public static String Mask(String inStr)
        {
            if (String.IsNullOrEmpty(inStr))
                return String.Empty;
            if (inStr.Length <= 4)
                return "".PadRight(inStr.Length, 'x');
            else if (inStr.Length == 5)
                return String.Format("{0}{1}", "".PadRight(4, 'x'), inStr.Substring(4, 1));
            else
            {
                int xCount = (int)(inStr.Length / 2); //number of x
                int sCount = (int) Math.Floor((decimal) (inStr.Length - xCount) / 2); //number of leading clear characters
                int tCount = (int)Math.Ceiling((decimal)(inStr.Length - xCount) / 2); //number of trailing clear characters

                return String.Format("{0}{1}{2}", inStr.Substring(0, sCount), "".PadRight(xCount, 'x'), inStr.Substring(inStr.Length - sCount - xCount + 1, tCount));
            }
        }
    }
}
