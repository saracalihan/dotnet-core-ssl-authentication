using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Certificate;

namespace WebApplication5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            /********/
            //services.Configure<HttpsConnectionAdapterOptions>(options =>
            //{
            //    options.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
            //    options.CheckCertificateRevocation = false;
            //    options.ClientCertificateValidation = (certificate2, chain, policyErrors) =>
            //    {
            //        // accept any cert (testing purposes only)
            //        return true;
            //    };
            //});

            services.AddAuthentication(
                 CertificateAuthenticationDefaults.AuthenticationScheme)
                    .AddCertificate(options =>
                    {
                        options.AllowedCertificateTypes = CertificateTypes.All;
                    });



            services.AddControllers();
            //services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
            //    .AddCertificate(/* options=> {
            //        OnCertificateValidated = context =>
            //        {
            //            var claims = new[]
            //            {
            //        new Claim(
            //            ClaimTypes.NameIdentifier,
            //            context.ClientCertificate.Subject,
            //            ClaimValueTypes.String,
            //            context.Options.ClaimsIssuer),
            //        new Claim(ClaimTypes.Name,
            //            context.ClientCertificate.Subject,
            //            ClaimValueTypes.String,
            //            context.Options.ClaimsIssuer)
            //    };

            //            context.Principal = new ClaimsPrincipal(
            //                new ClaimsIdentity(claims, context.Scheme.Name));
            //            context.Success();

            //            return Task.CompletedTask;
            //        }
            //    }*/)
            //    .AddCertificateCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCertificateMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
