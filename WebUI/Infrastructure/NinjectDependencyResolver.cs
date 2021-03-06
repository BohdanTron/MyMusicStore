﻿using Domain.Abstract;
using Domain.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IRepository>().To<EFRepository>();

            EmailSettings emailSettings = new EmailSettings();
            _kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("emailSettings", emailSettings);
        }
    }
}