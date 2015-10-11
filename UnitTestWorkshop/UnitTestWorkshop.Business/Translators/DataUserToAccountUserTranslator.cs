using SMG.Core.Contracts;
using UnitTestWorkshop.Business.Models.AccountModels;
using UnitTestWorkshop.Data.Models.AccountModels;

namespace UnitTestWorkshop.Business.Translators
{
    /// <summary>
    /// Business Requirements:
    /// 1. Must accurately translate a Data User to a User Account
    /// </summary>
    public class DataUserToAccountUserTranslator : ITranslate<User, UserAccount>
    {
        public UserAccount Translate(User source)
        {
            return Translate(source, new UserAccount());
        }

        public UserAccount Translate(User source, UserAccount existing)
        {
            existing.UserId = source.UserId;
            existing.Email = source.Email;
            existing.FirstName = source.FirstName;
            existing.LastName = source.LastName;

            return existing;
        }
    }
}
