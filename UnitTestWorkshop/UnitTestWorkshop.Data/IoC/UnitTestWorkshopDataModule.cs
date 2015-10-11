using Ninject.Modules;
using SMG.Core;
using SMG.Core.Data.Contracts;
using UnitTestWorkshop.Data.Models.AccountModels;
using UnitTestWorkshop.Data.Models.QueryModels;
using UnitTestWorkshop.Data.Repositories;

namespace UnitTestWorkshop.Data.IoC
{
    /// <summary>
    /// Business Requirements:
    /// 1. Should bind every contract to the concrete that uses it in this project.
    /// </summary>
    public class UnitTestWorkshopDataModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Rebind<ISystemTime>().To<SystemTime>();

            BindUserAuthenticationRepository();

            BindUserRepository();
        }

        private void BindUserRepository()
        {
            Kernel.Bind<ICreatable<User>>().To<UserRepository>();
            Kernel.Bind<IUpdatable<User>>().To<UserRepository>();
            Kernel.Bind<IDeletable<User>>().To<UserRepository>();
            Kernel.Bind<IRetrievable<ByUserEmail, User>>().To<UserRepository>();
            Kernel.Bind<IRetrievable<ByUserId, User>>().To<UserRepository>();
        }

        private void BindUserAuthenticationRepository()
        {
            Kernel.Bind<ICreatable<UserAuthentication>>().To<UserAuthenticationRepository>();
            Kernel.Bind<IUpdatable<UserAuthentication>>().To<UserAuthenticationRepository>();
            Kernel.Bind<IDeletable<UserAuthentication>>().To<UserAuthenticationRepository>();
            Kernel.Bind<IBulkRetrievable<ByEncodedUserId, UserAuthentication>>().To<UserAuthenticationRepository>();
        }
    }
}
