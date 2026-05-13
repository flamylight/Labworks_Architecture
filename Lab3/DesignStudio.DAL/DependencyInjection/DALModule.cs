using Autofac;
using DAL.Data;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.DependencyInjection;

public class DALModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //Repositories
        builder.RegisterGeneric(typeof(GenericRepository<>))
            .As(typeof(IGenericRepository<>))
            .InstancePerLifetimeScope();
        
        builder.RegisterType<OrderRepository>()
            .As<IGenericRepository<Order>>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<PackageRepository>()
            .As<IGenericRepository<Package>>()
            .InstancePerLifetimeScope();
        
        //Unit of Work
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
    }
}