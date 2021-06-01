using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using mbDirect.Vault.Repo;
using mbDirect.Vault.Models.Data;
using mbDirect.Vault.Models.Interface;
using mbDirect.Vault.Models.ExceptionType;
using mbDirect.Vault.API.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mbDirect.Vault.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private VaultContext _vault;

        public AccountController(VaultContext vault, ILogger<AccountController> logger)
        {
            _logger = logger;
            _vault = vault;
        }

        [HttpGet]
        public String Get()
        {
            _vault.Database.EnsureCreated();
            return "ok";
        }

        /// <summary>
        /// Get account data by AccountRequest.Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetAccount")]
        public ActionResult GetAccount([ModelBinder()] AccountRequest request)
        {
            try
            {
                Account acct = _vault.Accounts.Where(m => m.Id == request.id).FirstOrDefault();

                if (acct == null)
                    //return NotFound(new AccountException(AccountException.AccountExceptionType.NOT_FOUND));
                    throw new AccountException(AccountException.AccountExceptionType.NOT_FOUND);

                if (acct.InstrumentTypeCode == "credit" || acct.InstrumentTypeCode == "debit")
                {
                    return Ok(new Card(acct));
                }
                else
                    return Ok(acct);
            }
            catch (SqlException dex)
            {
                throw dex;
            }
        }

        /// <summary>
        /// Add a new account
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public ActionResult Post([FromBody] Account acct)
        {
            Account newAccount = new Account { InstrumentTypeCode = acct.InstrumentTypeCode, OwnerName = acct.OwnerName, Number = acct.Number };
            if ((newAccount.InstrumentTypeCode == "credit" || newAccount.InstrumentTypeCode == "debit"))
            {
                newAccount.BillingZipCode = acct.BillingZipCode;
                newAccount.Expiration = acct.Expiration;
            }
            else
            {
                newAccount.RoutingNumber = acct.RoutingNumber;
            }
            _vault.Accounts.Add(newAccount);
            _vault.SaveChanges();
            return Ok(newAccount.Id);

            throw new Exception("what's wrong");
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
        }
    }
}
