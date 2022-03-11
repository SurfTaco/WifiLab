using System;
using System.IO;
using System.Reflection;
using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Services;

namespace WebAPI
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

            // Hier adden wir dem Controller AddNewtonsoftJson (vorher haben wir
            // das Package Microsoft.AspnetCore.Mvc.NewtonsoftJson installiert)
            // damit sagen wir er soll doch bitte bei den autoatmischen Json
            // Serialisierungen / Deserialisierungen den Newtonsoft verwenden
            // und weiters geben wir ihm hier die Info / Setting mit, dass
            // er im Falle von Reference Loops keinen unendlichen String erzeugen soll.
            services.AddControllers()
                .AddNewtonsoftJson(
                    jsonOptions =>
                    {
                        // None -> damit nicht die ID's Objektzaehler mitesrialisiert werden.
                        jsonOptions.SerializerSettings.PreserveReferencesHandling =
                        Newtonsoft.Json.PreserveReferencesHandling.None;
                        // Sicherheitshalber eine Vorsichtsmaßnahme damit JsonSerializer
                        // nicht abstürzt wenn er auf einen Reference Loop trifft
                        jsonOptions.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    }

                );
            ;

            services.AddSwaggerGen(options =>
            {
                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                options.IncludeXmlComments(xmlPath);
            });

            // Für .Net 6 VS 2022
            // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // für .net 5/6 auf win bzw. vs 19 bzw. .net6 auf mac vs 19
            services.AddAutoMapper(typeof(Startup));

            // Wir registrieren den tokenGenerator als Service
            // damit er uns via Constructor Injection
            // überall zur Verfügung steht - automatisiert durch
            // das asp Framework
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            

            // Vorgefertigte Claims vom Framework löschen.
            // TODO: Obsolet?
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Registrierung Authentication und unseren Bearer Token
            services.AddAuthentication(opt =>
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme
            )
            .AddJwtBearer(cfg =>
            {
            cfg.SaveToken = true;
            // Bei Produktionbetrieb bitte immer true!
            cfg.RequireHttpsMetadata = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Configuration["JwTIssuer"],
                ValidAudience = Configuration["JwTIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(Configuration["JwTKey"])),
                ClockSkew = TimeSpan.Zero
                };
            });

            

            services.AddDbContext<LibContext>(
                options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("MyConString")
                ), ServiceLifetime.Scoped
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SwaggerUIConfig(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        void SwaggerUIConfig(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
        }
    }
}
