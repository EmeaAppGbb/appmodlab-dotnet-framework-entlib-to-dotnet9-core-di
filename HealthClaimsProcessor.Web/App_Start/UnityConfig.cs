using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Unity;
using Unity.Lifetime;
using Unity.AspNet.Mvc;
using HealthClaimsProcessor.Core.DataAccess;
using HealthClaimsProcessor.Core.Services;

namespace HealthClaimsProcessor.Web
{
    public static class UnityConfig
    {
        public static IUnityContainer RegisterComponents()
        {
            var container = new UnityContainer();

            // Repositories (singletons - parameterless constructors using DatabaseFactory)
            container.RegisterType<PatientRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ProviderRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ClaimRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<PaymentRepository>(new ContainerControlledLifetimeManager());

            // Services (transient - resolved with constructor-injected repositories)
            container.RegisterType<PatientService>();
            container.RegisterType<ProviderService>();
            container.RegisterType<ClaimService>();
            container.RegisterType<PaymentService>();

            // Enterprise Library ExceptionManager from config
            var configurationSource = ConfigurationSourceFactory.Create();
            var exceptionManager = new ExceptionPolicyFactory(configurationSource).CreateManager();
            container.RegisterInstance<ExceptionManager>(exceptionManager);

            // Set Unity as the MVC dependency resolver
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }
    }
}
