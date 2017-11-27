using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIResolver.Data
{
    public static class NinjectConfig
    {
        private static IKernel kernel;

        public static void SetupNinject()
        {
            kernel = new StandardKernel(new DIResolver.Data.MainResolver());

            var modules = new List<INinjectModule>
            {
                new DIResolver.Data.RepositoryResolver()
            };

            kernel.Load(modules);
        }

        public static IKernel GetKernal()
        {
            return kernel;
        }
    }
}