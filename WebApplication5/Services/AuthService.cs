using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using WebApplication5.Helpers;

namespace WebApplication5.Services.AuthService
{
    public class AuthService
    {
        private List<string> validThumbprints = new List<string>();
        public AuthService()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            foreach (X509Certificate2 cert in store.Certificates)
            {
                validThumbprints.Add(cert.Thumbprint);
            }
            string thumbPrints = string.Join(", ", validThumbprints.ToArray());
            LogHelper.LogError(string.Concat(LogHelper.LogType.Info," ", MethodBase.GetCurrentMethod().Name, " Thumbprints: ", thumbPrints ));
        }
        public bool authenticateCert(X509Certificate2 crt )
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            return validThumbprints.IndexOf(crt.Thumbprint) != -1 ? true : false;
        }
    }
}
