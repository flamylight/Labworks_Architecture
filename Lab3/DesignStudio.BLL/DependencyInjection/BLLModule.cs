using Autofac;
using BLL.Interfaces;
using BLL.Managers;

namespace BLL.DependencyInjection;

public class BLLModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //Managers
        builder.RegisterType<OrderManager>()
            .As<IOrderManager>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<PackageManager>()
            .As<IPackageManager>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<ServiceManager>()
            .As<IServiceManager>()
            .InstancePerLifetimeScope();
    }
}