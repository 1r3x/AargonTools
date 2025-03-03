using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AargonTools.Configuration;
using AargonTools.Data;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager;
using AargonTools.Manager.BackgroundJobs;
using AargonTools.Manager.GenericManager;
using AargonTools.Middleware;
using AargonTools.Models;
using AargonTools.Models.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using Serilog.Context;
using Swashbuckle.AspNetCore.Filters;
using AargonTools.Manager.ProcessCCManager;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace AargonTools
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


            services.Configure<ApplicationsOptions>(Configuration.GetSection("ApplicationOptions"));

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            //for centralize data 
            services.Configure<CentralizeVariablesModel>(Configuration.GetSection("CentralizeVariables"));

            services.AddDbContext<ApiDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

            services.AddDbContext<ExistingDataDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"), ServiceLifetime.Scoped);

            services.AddDbContext<TestEnvironmentDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:TestEnvironmentConnection"), ServiceLifetime.Scoped);

            services.AddDbContext<ProdOldDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:ProdOldConnection"), ServiceLifetime.Scoped);

            services.AddDbContext<CurrentBackupTestEnvironmentDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:CurrentBackupTestConnection"), ServiceLifetime.Scoped);




            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParams);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParams;
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                        .AddEntityFrameworkStores<ApiDbContext>();

            services.AddControllers();
            //IHttpContextAccessor register



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AargonTools", Version = "v1" });
                //register the filters
                c.ExampleFilters();
                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.OperationFilter<AuthResponsesOperationFilter>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
            //this service is for swagger example 
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();


            //service for background job

            services.AddHostedService<TestJob>();



            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            //api services from here 


            //injected common uses manages
            services.AddScoped<ResponseModel>();
            services.AddScoped<AdoDotNetConnection>();
            services.AddScoped<GetTheCompanyFlag>();
            services.AddScoped<GatewaySelectionHelper>();
            services.AddScoped<PostPaymentA>();

            //injected getAccountInformation v1.0
            services.AddScoped<IGetAccountInformation, GetAccountInformation>();
            //injected getAccountInformation v2.0
            services.AddScoped<IGetInteractionsAcctData, GetInteractionsAcctDataManager>();


            //injected setAccountInformation v1.0
            services.AddScoped<ISetMoveAccount, SetMoveAccount>();
            services.AddScoped<IAddNotes, AddNotes>();
            services.AddScoped<IAddNotesV3, AddNotesV3>();
            services.AddScoped<IAddBadNumbers, AddBadNumbers>();
            services.AddScoped<ISetDoNotCall, SetDoNotCall>();
            services.AddScoped<ISetNumber, SetNumberManager>();
            services.AddScoped<ISetMoveToHouse, SetMoveToHouseManager>();
            services.AddScoped<ISetMoveToDispute, SetMoveToDisputeManager>();
            services.AddScoped<ISetPostDateChecks, SetPostDateChecksManager>();
            services.AddScoped<IProcessCcPayment, ProcessCcPaymentManager>();
            services.AddScoped<ISetCCPayment, SetCCPaymentManager>();
            services.AddScoped<ISetBlandResults, SetBlandResultsManager>();


            //injected HrmData v1.0
            services.AddScoped<IGetHrm, GetHrmManager>();
            services.AddScoped<ISetEmployeeTimeLogEntry, SetHrmManager>();
            services.AddScoped<ISetMoveToQueue, SetMoveToQueueManager>();
            services.AddScoped<IAddNotesV2, AddNotesV2Manager>();
            services.AddScoped<ISetDialing, SetDialingManager>();
            services.AddScoped<ISetUpdateAddress, SetUpdateAddressManager>();
            services.AddScoped<IAddCcPaymentV2, AddCcPaymentV2>();
            services.AddScoped<ICardTokenizationDataHelper, CardTokenizationDataHelper>();
            services.AddScoped<ICryptoGraphy, CryptoGraphy>();
            services.AddScoped<IViewingSchedulePayments, ViewingSchedulePayments>();
            //
            //injected cc process v1.0
            services.AddHttpClient<IUniversalCcProcessApiService, UniversalCcProcessApiService>();
            services.AddScoped<IPreSchedulePaymentProcessing, PreSchedulePaymentProcessingManager>();
            //injected cc process v2.0
            services.AddTransient<UsaEPayManager>();
            services.AddTransient<ElavonManager>();
            services.AddTransient<InstaMedManager>();
            services.AddTransient<IProClassManager>();
            services.AddTransient<TmcElavonManager>();
            services.AddScoped<PaymentGatewayFactory>();

            //
            services.AddHttpContextAccessor();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISetInteractResults, InteractResultsManager>();


            //for VPN lookup
            services.AddHttpClient();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Log Request Body
                app.Use(async (context, next) =>
                {
                    context.Request.EnableBuffering(); // Enable rewinding the request stream
                    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    Serilog.Log.Debug("Request body: {RequestBody}", requestBody);
                    context.Request.Body.Position = 0; // Reset the stream position
                    await next();
                });
            }


            // global exceptionalhandler 
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionHandlerFeature?.Error;

                    if (exception is ConnectionResetException)
                    {
                        Serilog.Log.Warning("Client disconnected (global ex handler): {Message}", exception.Message);
                        context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                        await context.Response.WriteAsync("Client disconnected");

                    }
                    else
                    {
                        Serilog.Log.Error(exception, "Unhandled exception (global ex handler)");
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred");
                    }
                });
            });






            //app.UseIPFilter();

            //Apply IP filtering middleware conditionally
            app.UseWhen(context => !context.Request.Path.StartsWithSegments("/no-ip-filter"), appBuilder =>
            {
                appBuilder.UseIPFilter();
            });

            // Catch Model Binding Errors
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (ConnectionResetException ex)
                {
                    Serilog.Log.Warning("Client disconnected (model binding ex): {Message}", ex.Message);
                    context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                    await context.Response.WriteAsync("Client disconnected");

                }
            });

            // Security Headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                await next();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AargonTools v1"));
            app.UseHttpsRedirection();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<RateLimitingMiddleware>();



            //this is for picking up the user and send it to log.
            app.Use(async (httpContext, next) =>
            {
                var claims = httpContext.User.Claims;
                var emailCheck = "";
                foreach (var x in claims)
                {
                    if (!x.Value.Contains("@")) continue;
                    var eMailAddress = new System.Net.Mail.MailAddress(x.Value);
                    emailCheck = eMailAddress.Address;

                }


                var email = httpContext.User.Identity.IsAuthenticated ? emailCheck : "anonymous";

                LogContext.PushProperty("UserName", email);

                await next.Invoke();
            });


            app.UseCors("Open");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }



    }

    internal class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                .Union(context.MethodInfo.GetCustomAttributes(true));

            if (attributes.OfType<IAllowAnonymous>().Any())
            {
                return;
            }

            var authAttributes = attributes.OfType<IAuthorizeData>();

            if (authAttributes.Any())
            {
                operation.Responses["401"] = new OpenApiResponse { Description = "Unauthorized" };

                if (authAttributes.Any(att => !String.IsNullOrWhiteSpace(att.Roles) || !String.IsNullOrWhiteSpace(att.Policy)))
                {
                    operation.Responses["403"] = new OpenApiResponse { Description = "Forbidden" };
                }

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "BearerAuth",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                };
            }
        }
    }
}
