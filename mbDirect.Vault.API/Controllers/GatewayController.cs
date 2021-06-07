using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mbDirect.Vault.Repo;
using mbDirect.Vault.Models.Data;
using mbDirect.Vault.Models.Interface;
using mbDirect.Vault.Models.ExceptionType;
using Microsoft.Extensions.Logging;

namespace mbDirect.Vault.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;
        private VaultContext _vault;

        public GatewayController(VaultContext vault, ILogger<GatewayController> logger)
        {
            _logger = logger;
            _vault = vault;
        }
        [HttpGet("Gateways")]
        public ActionResult Gateways()
        {
            var gateways = _vault.Gateways;

            return Ok(gateways);
        }

        [HttpPost("Gateway")]
        public ActionResult Gateway([ModelBinder()] Gateway request)
        {
            if (_vault.Gateways.Where(p=>p.EndPoint.ToLower() == request.EndPoint.ToLower()).Count() > 0)
                throw new RequestException(RequestException.RequestExceptionType.ALREADY_EXISTS);

            Gateway gw = new Gateway { Description = request.Description, EndPoint = request.EndPoint };
            _vault.Gateways.Add(gw);
            _vault.SaveChanges();

            if (gw.Id > 0)
                return Ok(gw.Id);
            else 
                throw new RequestException(RequestException.RequestExceptionType.INVALID_REQUEST);
        }

        [HttpGet("Users")]
        public ActionResult Users()
        {
            List<TransitCredential> users = _vault.TransitCredentials.Include(m=>m.TransitGateway).ToList();
            foreach (var u in users)
            {
                u.PasswordEncrypted = "".PadRight(20, 'x');
            }
               
            return Ok(users);
        }
        [HttpPost("User")]
        public ActionResult User([ModelBinder] TransitCredentialRequest request)
        {
            if (_vault.TransitCredentials.Where(m=>m.UserId.ToLower() == request.UserId.ToLower()).Count() > 0)
                throw new RequestException(RequestException.RequestExceptionType.ALREADY_EXISTS);

            if (request.TransitGateway.Id <= 0)
                throw new RequestException(RequestException.RequestExceptionType.INVALID_REQUEST);
            else if (_vault.Gateways.Where(m=>m.Id == request.TransitGateway.Id).Count() == 0)
                throw new RequestException(RequestException.RequestExceptionType.INVALID_REQUEST, "Invalid Gateway Endpoint");

            String encKey = Service.Crypto.CreateRandomText(16);
            String encVector = Service.Crypto.CreateRandomText(16);
            String encPassword = Service.Crypto.EncryptString(request.Password, encKey, encVector, Service.Crypto.TypeAlgorithm.AES);

            Key key = new Key { KeyValue = encKey, Vector = encVector };
            _vault.Keys.Add(key);
            _vault.SaveChanges();
            TransitCredential user = new TransitCredential();
            user.UserId = request.UserId;
            user.DeveloperId = request.DeveloperId;
            user.PasswordEncrypted = encPassword;
            user.PasswordKeyId = key.KeyId;
            user.TransitGateway = _vault.Gateways.Where(m => m.Id == request.TransitGateway.Id).First();
            _vault.TransitCredentials.Add(user);
            _vault.SaveChanges();

            return Ok(user.Number);
        }

        [HttpGet("Merchants")]
        public ActionResult Merchants()
        {
            var merchants = _vault.Merchants.Include(m => m.TransitUser).ToList();
            foreach (var m in merchants)
            {
                m.TransitUser.PasswordEncrypted = "".PadRight(20, 'x');
                m.TransitTransactionKey = Service.Utility.Mask(m.TransitTransactionKey);
            }

            return Ok(merchants);
        }

        [HttpPost("Merchant")]
        public ActionResult Merchant([ModelBinder] MerchantRequest request)
        {
            TransitCredential user = _vault.TransitCredentials.Where(m => m.Number == request.TransitUserNumber).Include(m=>m.TransitGateway).FirstOrDefault();

            if (user == null)
                throw new RequestException(RequestException.RequestExceptionType.INVALID_REQUEST, "Invalid Transit User Number");

            Key key = _vault.Keys.Where(m => m.KeyId == user.PasswordKeyId).FirstOrDefault();
            if (key == null)
                throw new RequestException(RequestException.RequestExceptionType.UNEXPECTED, "Invalid Encryption Key in Transit User");

            String pw = Service.Crypto.DecryptString(user.PasswordEncrypted, key.KeyValue, key.Vector, Service.Crypto.TypeAlgorithm.AES);

            throw new NotImplementedException();
        }


        [HttpPost("Verify")]
        public ActionResult Verify()
        {
            throw new NotImplementedException();
        }

        [HttpPost("Sale")]
        public ActionResult Sale()
        {
            throw new NotImplementedException();
        }
    }
}
