using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Services.AuthService;


namespace WebApplication5
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CertificateMiddleware
    {
        private readonly RequestDelegate _next;

        public CertificateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            Console.WriteLine("gectii");
            // kullanıcı dogrulamasi burada yapilmali ve veri icine kullanici bilgisi eklenmeli
            AuthService AuthService = new AuthService();
            X509Certificate2 clientCertifacate = httpContext.Connection.ClientCertificate;
            if(clientCertifacate == null)
            {
  		        //await BufferRequestBodyAsync();
 	            clientCertifacate = await httpContext.Connection.GetClientCertificateAsync();
		    
                if(clientCertifacate == null)
                    throw new ArgumentException("sertifika bulunamadı");
            }
            var user = AuthService.authenticateCert(clientCertifacate);

            Console.WriteLine(user);
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
