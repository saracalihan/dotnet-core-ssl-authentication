using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Services.AuthService;
using System.Reflection;
using WebApplication5.Helpers;

namespace WebApplication5
{
    public class CertificateMiddleware
    {
        private readonly RequestDelegate _next;

        public CertificateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            // kullanıcı dogrulamasi burada yapilmali ve veri icine kullanici bilgisi eklenmeli
            AuthService AuthService = new AuthService();
            X509Certificate2 clientCertifacate = httpContext.Connection.ClientCertificate;
            //string clientCertFromHeader = httpContext.Request.Headers["X-ARR-ClientCert"];
            //Console.WriteLine(clientCertFromHeader);
            LogHelper.LogError(string.Concat(LogHelper.LogType.Info, " ", MethodBase.GetCurrentMethod().Name, " Client Thumbprint: ", clientCertifacate.Thumbprint));
            if (clientCertifacate == null)
            {
  		        // await BufferRequestBodyAsync();
 	            clientCertifacate = await httpContext.Connection.GetClientCertificateAsync();
		    
                if(clientCertifacate == null)
                    throw new Exception("sertifika alınamadı");
            }
            if (!AuthService.authenticateCert(clientCertifacate))
            {
                throw new Exception("sertifika doğrulanamadı Thumbprint: " + clientCertifacate.Thumbprint );
            }
            LogHelper.LogError(string.Concat(LogHelper.LogType.Info, " ", MethodBase.GetCurrentMethod().Name, " Doğrulama başarılı"));
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CertificateMiddlewareExtensions
    {
        public static IApplicationBuilder UseCertificateMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CertificateMiddleware>();
        }
    }
}
