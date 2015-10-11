using UnitTestWorkshop.Business.Models.AccountModels;

namespace UnitTestWorkshop.Business.Providers
{
    public interface IUserAuthenticationProvider
    {
        void SetupNewCredentials(UserAccount userAccount, Credentials credentials);
        AuthenticationResult Authenticate(Credentials credentials);
        AuthenticationResult ChangePassword(UpdateCredentials credentials);
        void DeleteCredentials(Credentials credentials);
    }
}