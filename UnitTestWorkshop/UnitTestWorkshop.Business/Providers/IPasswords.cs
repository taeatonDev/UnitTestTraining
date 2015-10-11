namespace UnitTestWorkshop.Business.Providers
{
    public interface IPasswords
    {
        string GeneratePassword(int length, int numberOfNonAlphanumericCharacters);
    }
}