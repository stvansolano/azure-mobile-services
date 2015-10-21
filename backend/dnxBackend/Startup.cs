namespace Backend
{
    using Microsoft.AspNet.Authentication;
    using Microsoft.AspNet.Authentication.Twitter;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Diagnostics;
    using Microsoft.AspNet.Diagnostics.Entity;
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.Identity;
    using Microsoft.Framework.Configuration;
    using Microsoft.Framework.DependencyInjection;
    using Microsoft.Framework.Logging;
    using Microsoft.Framework.Runtime;

    public partial class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.

            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure the options for the authentication middleware.
            // You can add options for Google, Twitter and other middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            /*services.Configure<FacebookAuthenticationOptions>(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.Configure<MicrosoftAccountAuthenticationOptions>(options =>
            {
                options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
                options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            });*/

            // Register application services.

            string connectionString;
            Configuration.TryGet("MicrosoftAzureStorage:ConnectionString", out connectionString);
            var context = new CloudContext(connectionString);

            string blobContainer;
            Configuration.TryGet("MicrosoftAzureStorage:BlobContainer", out blobContainer);
            var blobStorage = new MediaStorageRepository(blobContainer, context);

            services.AddSingleton<CloudContext, CloudContext>().AddInstance(context);
            services.AddSingleton<MediaStorageRepository, MediaStorageRepository>().AddInstance(blobStorage);
            services.AddSingleton<UserRepository, UserRepository>();
            services.AddSingleton<AccountRepository, AccountRepository>();
            services.AddSingleton<MomentRepository, MomentRepository>();

            services.AddTransient<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory>();
            services.AddSingleton<IdentityErrorDescriber, IdentityErrorDescriber>();
            services.AddTransient<ILookupNormalizer, LookupNormalizer>();
            services.AddTransient<IPasswordHasher<User>, ClearPassword>();
            services.AddSingleton<IUserStore<User>, CustomUserStore>();
            services.AddTransient<UserManager<User>, UserManager<User>>();

            services.AddTransient<SignInManager<User>, SignInManager<User>>();

            /*
            services.Configure<TwitterAuthenticationOptions>(options =>
            {
                options.ConsumerKey = "Authentication:TwitterAccount:ConsumerKey";
                options.ConsumerSecret = "Authentication:TwitterAccount:ConsumerSecret";
                options.SignInScheme = new TwitterAuthenticationOptions().SignInScheme;
            });*/

            // Add MVC services to the services container.
            services.AddMvc();

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();

            // Configure the HTTP request pipeline.

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            app.UseIdentity();

            // Add authentication middleware to the request pipeline. You can configure options such as Id and Secret in the ConfigureServices method.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            // app.UseFacebookAuthentication();
            // app.UseGoogleAuthentication();
            // app.UseMicrosoftAccountAuthentication();
            //app.UseTwitterAuthentication();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}