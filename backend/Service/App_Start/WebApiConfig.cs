
namespace Backend
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web.Http;
    using Backend.DataObjects;
    using Backend.Models;
    using Microsoft.WindowsAzure.Mobile.Service;
    using Autofac;
    using System.Configuration;

    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            HttpConfiguration config = RegisterIoC(options);

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.SetIsHosted(true);

            //var mobileServiceName = ConfigurationManager.AppSettings[ServiceSettingsKeys.ServiceName];
            //var myTraceWriter = new MyTraceWriter(originalTraceWriter, mobileServiceName, storageConnectionString);
            //config.Services.Replace(typeof(ITraceWriter), myTraceWriter);

            Database.SetInitializer(new MobileServiceInitializer());
        }

        private static HttpConfiguration RegisterIoC(ConfigOptions options)
        {
            var storageConnection = ConfigurationManager.ConnectionStrings["TableStorageConnectionString"].ConnectionString;
            var mediaContainer = ConfigurationManager.AppSettings["BlobContainer"];
            var context = new CloudContext(storageConnection);

            var mediaRepository = new MediaStorageRepository(mediaContainer, context);
            var momentRepository = new MomentRepository(context);

            ConfigBuilder builder = new ConfigBuilder(options, (httpConfig, autofac) =>
            {
                autofac.RegisterInstance(context);
                autofac.RegisterInstance(mediaRepository);
                autofac.RegisterInstance(momentRepository);
                autofac.RegisterInstance(new UserRepository(context));
            });

            var config = ServiceConfig.Initialize(builder);
            // Set the dependency resolver to be Autofac.
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return config;
        }
    }

    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (var todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}

