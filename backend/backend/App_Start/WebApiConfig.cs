using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Backend.DataObjects;
using Backend.Models;
using Microsoft.Azure.Mobile.Server.Config;

namespace Backend
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            HttpConfiguration config = new HttpConfiguration();

            new MobileAppConfiguration()
                //.UseDefaultConfiguration()
                .ApplyTo(config);

            /*HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options, (httpConfig, autofac) =>
            {
                autofac.RegisterInstance(new MyDependency()).As<IMyDependencyInterface>();
            }));
            */

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}