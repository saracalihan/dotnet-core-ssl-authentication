using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace WebApplication5.Services.AuthService
{
    public class AuthService
    {
        public string authenticateCert(X509Certificate2 crt )
        {
            return crt.Subject;
        }
    }
}
