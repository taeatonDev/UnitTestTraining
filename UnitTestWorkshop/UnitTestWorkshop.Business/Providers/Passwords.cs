using System.Web.Security;

namespace UnitTestWorkshop.Business.Providers
{
    /// <summary>
    /// Business Requirements:
    /// 1. Provide a generated password
    /// 2. Generate a password of a given length
    /// 3. Generate a password with a given number of non alphanumeric characters
    /// </summary>
    public class Passwords : IPasswords
    {
        public string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            return Membership.GeneratePassword(length, numberOfNonAlphanumericCharacters);
        }
    }
}