using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http.Features;
using mbDirect.Vault.Models.Data;
using mbDirect.Vault.Models.Interface;


namespace mbDirect.Vault.API.Filters
{
    public class VaultModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var content = GetBodyContent(bindingContext);
            var contentType = bindingContext.HttpContext.Request.ContentType;

            if (!String.IsNullOrEmpty(content))
            {
                bindingContext.Model = GetModel(bindingContext.ModelType, content, contentType);
                bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            }

            return Task.CompletedTask;
        }
        /// <summary>
        /// Get request body from HttpContext
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        protected String GetBodyContent(ModelBindingContext bindingContext)
        {
            var httpBodyControl = bindingContext.HttpContext.Features.Get<IHttpBodyControlFeature>();
            httpBodyControl.AllowSynchronousIO = true;

            var content = String.Empty;
            using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                content = reader.ReadToEnd();
                return content;
            }
        }

        #region protected Deserializer
        public virtual object GetModel(Type t, String content, String contentType)
        {
            if (t == typeof(Account))
                return this.Deserialize<Account>(content, contentType);
            else if (t == typeof(AccountRequest))
                return this.Deserialize<AccountRequest>(content, contentType);

            return null;
        }

        protected virtual T Deserialize<T>(String content, String contentType)
        {
            if (String.IsNullOrEmpty(content)) //no content, returns null or default value of the type T
                return default(T);

            if ((contentType.IndexOf("xml") >= 0)) //xml based input 
                return this.DeserializeXml<T>(content);

            else if (contentType.IndexOf("json") >= 0) //json based input
                return this.DeserializeJson<T>(content);

            return default(T);
        }
        /// <summary>Deserialize using Xml Serializer
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual T DeserializeXml<T>(String content)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(content);
                if (xdoc.DocumentElement.Attributes != null)
                    if (xdoc.DocumentElement.Attributes.GetNamedItem("xmlns") != null)
                        if (xdoc.DocumentElement.Attributes.GetNamedItem("xmlns").InnerText.IndexOf("datacontract") >= 0)
                            return this.DeserializeDataContract<T>(content);

                //Input data not serialized using data contract serializer, use Xml Serializer
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (MemoryStream m = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    m.Seek(0, SeekOrigin.Begin);
                    T req = (T)serializer.Deserialize(m);

                    return req;
                }
            }
            catch (SerializationException lex)
            {

            }
            catch (XmlException xex)
            {
            }
            catch (Exception ex)
            {
            }

            return default(T);
        }
        /// <summary>Deserialize using DataContractSerializer
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual T DeserializeDataContract<T>(String content)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                using (MemoryStream m = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    m.Seek(0, SeekOrigin.Begin);
                    Object o = serializer.ReadObject(m);
                    if (o.GetType().IsAssignableFrom(typeof(T)))
                        return (T)o;
                }
            }
            catch (InvalidDataContractException lex)
            {

            }
            catch (Exception ex)
            {
            }
            return default(T);
        }
        /// <summary>Deserialize using JsonConverter
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual T DeserializeJson<T>(String content)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (JsonSerializationException jex)
            {
            }
            catch (Exception ex)
            {
            }

            return default(T);
        }
        #endregion
    }
}
