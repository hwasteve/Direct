using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.ExceptionType
{
    public class AccountException : BaseException
    {
        public enum AccountExceptionType
        {
            SUCCESS = 0, NOT_FOUND = 1, INVALID_DATA = 2
        }

        public AccountExceptionType Type { get; set; }

        #region Constructors
        public AccountException(AccountExceptionType t): this(t, "")
        {
        }
        public AccountException(AccountExceptionType t, String message) : base(message)
        {
            Type = t;
        }
        #endregion

        /// <summary>
        /// Get Message
        /// </summary>
        public override string Message 
        {
            get
            {
                if (!String.IsNullOrEmpty(base.Message))
                    return base.Message;
                else
                {
                    switch (Type)
                    {
                        case AccountExceptionType.INVALID_DATA:
                            return "Invalid Account Data";
                        case AccountExceptionType.NOT_FOUND:
                            return "Account Not Found";
                        case AccountExceptionType.SUCCESS:
                            return "";
                        default:
                            return "Account Exception";
                    }
                }
            }
        }
        /// <summary>
        /// Get Exception Code used in http response
        /// </summary>
        public override String Code
        {
            get
            {
                return Type.ToString();
            }
        }
        /// <summary>
        /// Get the HttpStatusCode 
        /// </summary>
        public override HttpStatusCode HttpStatus
        {
            get
            {
                switch (Type)
                {
                    case AccountExceptionType.INVALID_DATA:
                        return HttpStatusCode.BadRequest;
                    case AccountExceptionType.NOT_FOUND:
                        return HttpStatusCode.NotFound;
                    case AccountExceptionType.SUCCESS:
                        return HttpStatusCode.OK;
                    default:
                        return HttpStatusCode.BadRequest;
                }
            }
        }
    }
}
