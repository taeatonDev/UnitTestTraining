namespace UnitTestWorkshop.Business.Models.AccountModels
{
    public class AuthenticationResult
    {
        public bool WasSuccessful { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
