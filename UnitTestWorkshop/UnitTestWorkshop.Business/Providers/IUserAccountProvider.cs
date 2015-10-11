using UnitTestWorkshop.Business.Models.AccountModels;

namespace UnitTestWorkshop.Business.Providers
{
    public interface IUserAccountProvider
    {
        UserAccount CreateAccount(UserAccount newAccount, Credentials newCredentials);
        UserAccount RetrieveAccount(Credentials existingCredentials);
        UserAccount UpdateAccount(UserAccount existingAccount, Credentials existCredentials);
        void DeleteAccount(UserAccount existingAccount, Credentials existCredentials);
    }
}