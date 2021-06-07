using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.ExceptionType
{
    public class RequestException: BaseException
    {
        public enum RequestExceptionType { SUCCESS = 0, INVALID_REQUEST = 1, MISSING_DATA = 2, ALREADY_EXISTS = 4, UNEXPECTED = 65636}
    
        public RequestExceptionType Type { get; set; }

        #region Constructors
        public RequestException(RequestExceptionType t) : this(t, "")
        {
        }
        public RequestException(RequestExceptionType t, String message) : base(message)
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
                        case RequestExceptionType.INVALID_REQUEST:
                            return "Invalid Request";
                        case RequestExceptionType.MISSING_DATA:
                            return "Missing Data";
                        case RequestExceptionType.ALREADY_EXISTS:
                            return "Already Exists";
                        default:
                            return "Request Exception";
                    }
                }
            }
        }

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
                    case RequestExceptionType.INVALID_REQUEST:
                    case RequestExceptionType.MISSING_DATA:
                        return HttpStatusCode.BadRequest;
                    case RequestExceptionType.ALREADY_EXISTS:
                        return HttpStatusCode.Conflict;
                    case RequestExceptionType.UNEXPECTED:
                        return HttpStatusCode.InternalServerError;
                    case RequestExceptionType.SUCCESS:
                        return HttpStatusCode.OK;
                    default:
                        return HttpStatusCode.BadRequest;
                }
            }
        }
    }
}
