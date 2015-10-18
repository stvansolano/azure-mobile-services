using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Tables.Config;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;
using System.Web.Http;

[assembly: OwinStartup(typeof(Backend.Startup))]

namespace Backend
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            new MobileAppConfiguration()
               .AddMobileAppHomeController()
               .MapApiControllers()
               .AddTables(
                   new MobileAppTableConfiguration()
                       .MapTableControllers()
                       .AddEntityFramework()
                   )
               .AddPushNotifications()
               .MapLegacyCrossDomainController()
               .ApplyTo(config);
            app.UseWebApi(config);
            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());
        }
    }
}