using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mbDirect.Vault.Repo;
using mbDirect.Vault.Models.Data;
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

        [HttpPost("[Controller]/Verify")]
        public ActionResult Verify()
        {
            throw new NotImplementedException();
        }

        [HttpPost("[Controller]/Sale")]
        public ActionResult Sale()
        {
            throw new NotImplementedException();
        }
    }
}
