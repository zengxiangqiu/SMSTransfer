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

            builder.RegisterType<SmsLogsRepository>().AsSelf()
                .WithParameter("appkey", "")
                .WithParameter("projectId", "");

            builder.Register<SmsUserRepository>(c=>new SmsUserRepository("","")).AsSelf();
            builder.RegisterType<SmsLocalRepository>()
                .As<ISmsRepository>()
                .WithParameter("appkey", "")
                .WithParameter("projectId", "");
            builder.RegisterType<SmsService>().AsImplementedInterfaces();

            builder.Register(c =>  LogManager.GetCurrentClassLogger()).As<ILogger>();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var config = GlobalConfiguration.Configuration;

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter()
            //{
            //    DateTimeFormat = "yyyy-MM-dd hh:mm:ss"
            //});

        }
    }
}