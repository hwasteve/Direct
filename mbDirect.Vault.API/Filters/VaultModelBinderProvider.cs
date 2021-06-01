using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using mbDirect.Vault.Models.Data;
using mbDirect.Vault.Models.Interface;

namespace mbDirect.Vault.API.Filters
{
    public class VaultModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (typeof(Account) == context.Metadata.ModelType)
                return new VaultModelBinder();

            if (typeof(AccountRequest) == context.Metadata.ModelType)
                return new VaultModelBinder();

            return null;
        }
    }
}
