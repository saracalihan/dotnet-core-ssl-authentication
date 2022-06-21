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
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;

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

            services.Configure<HttpsConnectionAdapterOptions>(options =>
            {
                options.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                options.CheckCertificateRevocation = false;
                options.ClientCertificateValidation = (certificate2, chain, policyErrors) =>
                {
                    // accept anyrt (testing purposes only)
                    return true;
                };
            });

            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                    .AddCertificate(options =>
                    {
                        options.AllowedCertificateTypes = CertificateTypes.All;
                        options.Events = new CertificateAuthenticationEvents
                        {
                            OnCertificateValidated = context =>
                            {
                                //Leveraging the DP injected single instance of the CertificateValidation Service

                                // Here it validate and call the methode ValidateCertificate from MyCertValidation class.
                                if (true)
                                {
                                    context.Success();
                                }
                                else
                                {
                                    context.Fail("Invalid certificate");
                                }
                                return Task.CompletedTask;
                            },
                            OnAuthenticationFailed = context =>
                            {
                                context.Fail("Invalid certificate");
                                return Task.CompletedTask;
                            }
                        };
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
                        //    })
                    })
                                .AddCertificateCache();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    await context.Response.WriteAsync("Hello World!");
                    return;
                }

                await next();
            });

            app.UseCertificateMiddleware();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
        }
    }
}
