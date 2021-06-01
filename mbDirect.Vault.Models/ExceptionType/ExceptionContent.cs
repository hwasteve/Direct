using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.ExceptionType
{
    public class ExceptionContent
    {
        public ExceptionContent(BaseException ex)
        {
            From(ex);
        }
        public String Code { get; set; }
        public String Message { get; set; }
        public String[] Data { get; set; }

        public void From(Exception ex)
        {
            if (typeof(BaseException).IsAssignableFrom(ex.GetType()))
            {
                var e = ex as BaseException;
                Code = e.Code;
                Message = e.Message;
                Data = e.Data;
            } 
        }
    }
}
