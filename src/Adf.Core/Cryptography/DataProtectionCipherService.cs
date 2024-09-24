using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace Adf.Core.Cryptography
{
    public class DataProtectionCipherService : IDataProtectionCipherService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "my-very-long-key-of-no-exact-size";

        public DataProtectionCipherService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(input);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Unprotect(cipherText);
        }
    }
}
