using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace mbDirect.Vault.Models.ExceptionType
{
    public class BaseException : Exception
    {
        public String[] _data { get; private set; }

        #region Constructors
        public BaseException() : this("")
        {
        }
        public BaseException(params String[] data): this("", data)
        {

        }
        public BaseException(String message) : base(message)
        {
        }
        public BaseException(String message, params String[] data) : this(message)
        {
            _data = data;
        }
        #endregion

        public virtual String Code { get; set; }
        public virtual HttpStatusCode HttpStatus { get; set; }
        public new String[] Data
        {
            get
            {
                return _data;
            }
        }


        public virtual string ToJson()
        {
            ExceptionContent content = new ExceptionContent(this);

            return JsonConvert.SerializeObject(content);
        }
        public virtual String ToXml()
        {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(this.GetType());
            using (var stream = new StringWriter())
            {
                s.Serialize(stream, this);
                return stream.ToString();
            }
        }

        public virtual StringContent ToStringContent(String mediaType)
        {
            if (mediaType.IndexOf("xml") >= 0)
            {
                return new StringContent(ToXml(), Encoding.UTF8, "application/xml");
            }
            else
                return ToStringContent();
            
        }
        public virtual StringContent ToStringContent()
        {
            return new StringContent(ToJson(), Encoding.UTF8, "application/json");
        }
    }
}
