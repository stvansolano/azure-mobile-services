using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Tables.Config;
using System.Data.Entity;

[assembly: OwinStartup(typeof(Backend.Startup))]

namespace Backend
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            new MobileAppConfiguration()
               .AddMobileAppHomeController()             // from the Home package
               .MapApiControllers()
               .AddTables(                               // from the Tables package
                   new MobileAppTableConfiguration()
                       .MapTableControllers()
                       .AddEntityFramework()             // from the Entity package
                   )
               .AddPushNotifications()                   // from the Notifications package
               .MapLegacyCrossDomainController()         // from the CrossDomain package
               .ApplyTo(config);

            app.UseWebApi(config);
            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());
        }
    }
}
