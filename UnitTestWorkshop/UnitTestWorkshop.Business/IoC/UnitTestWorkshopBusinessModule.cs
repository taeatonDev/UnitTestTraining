using Ninject.Modules;
using SMG.Core;
using SMG.Core.Contracts;
using UnitTestWorkshop.Business.Models.AccountModels;
using UnitTestWorkshop.Business.Providers;
using UnitTestWorkshop.Business.Translators;
using UnitTestWorkshop.Data.Models.AccountModels;

namespace UnitTestWorkshop.Business.IoC
{
    /// <summary>
    /// Business Requirements:
    /// 1. Should bind every contract to the concrete that uses it in this project.
    /// </summary>
    public class UnitTestWorkshopBusinessModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Rebind<ISystemTime>().To<SystemTime>();

            BindHandlers();
            BindProviders();
            BindTranslators();
        }

        private void BindTranslators()
        {
            Kernel.Bind<ITranslate<User, UserAccount>>().To<DataUserToAccountUserTranslator>();
        }

        private void BindProviders()
        {
            Kernel.Bind<IPasswords>().To<Passwords>();
            Kernel.Bind<IUserAuthenticationProvider>().To<UserAuthenticationProvider>();
            Kernel.Bind<IUserAccountProvider>().To<UserAccountProvider>();
        }

        private void BindHandlers()
        {
        }
    }
}
