using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSTransfer.Repositories;
using SMSTransfer.Services;
using NLog;
using Autofac.Integration.Mvc;
using System.Reflection;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;


namespace SMSTransfer.Web.App_Start
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(SmsLogsRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsSelf()
               .WithParameter("appkey", "82f10ec6236942e390d16e909a4e9be5")
                .WithParameter("projectId", "2100");

            builder.RegisterType<SmsLocalRepository>()
                .As<ISmsRepository>()
                .WithParameter("appkey", "82f10ec6236942e390d16e909a4e9be5")
                .WithParameter("projectId", "2100");

            builder.RegisterType<SmsService>().AsImplementedInterfaces();

            builder.Register(c => LogManager.GetCurrentClassLogger()).As<ILogger>();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterControllers(typeof(WebApiApplication).Assembly);


            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var config = GlobalConfiguration.Configuration;

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            //config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter()
            //{
            //    DateTimeFormat = "yyyy-MM-dd hh:mm:ss"
            //});

        }
    }
}