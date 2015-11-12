using System;
using SMG.Core;
using SMG.Core.Contracts;
using SMG.Core.Data.Contracts;
using UnitTestWorkshop.Business.Models.AccountModels;
using UnitTestWorkshop.Data.Models.AccountModels;
using UnitTestWorkshop.Data.Models.QueryModels;

namespace UnitTestWorkshop.Business.Providers
{
    /// <summary>
    /// Business Requirements:
    /// 1. Must provide CRUD functionality for user accounts
    /// 2. Must keep Authentication in mind with RUD functionality.
    /// 3. Email Addresses must be unique across all users.
    /// 4. Account Creation must also address creation of Authentication
    /// </summary>
    public class UserAccountProvider : IUserAccountProvider
    {
        private readonly ICreatable<User> _userCreator;
        private readonly ITranslate<User, UserAccount> _translateDataUserToUserAccount;
        private readonly IUserAuthenticationProvider _userAuthProvider;
        private readonly IUpdatable<User> _userUpdater;
        private readonly IRetrievable<ByUserId, User> _userGetByUserId;
        private readonly IRetrievable<ByUserEmail, User> _retrieveUserByEmail;
        private readonly IDeletable<User> _userDeleter;
        private readonly ISystemTime _systemTime;

        public UserAccountProvider(
            IUserAuthenticationProvider userAuthProvider, 
            ICreatable<User> userCreator, 
            IRetrievable<ByUserId, User> userGetByUserId, 
            IRetrievable<ByUserEmail, User> retrieveUserByEmail, 
            IDeletable<User> userDeleter, 
            IUpdatable<User> userUpdater, 
            ITranslate<User, UserAccount> translateDataUserToUserAccount, 
            ISystemTime systemTime)
        {
            _userAuthProvider = userAuthProvider;
            _userCreator = userCreator;
            _userGetByUserId = userGetByUserId;
            _retrieveUserByEmail = retrieveUserByEmail;
            _userDeleter = userDeleter;
            _userUpdater = userUpdater;
            _translateDataUserToUserAccount = translateDataUserToUserAccount;
            _systemTime = systemTime;
        }

        /// <summary>
        /// Business Requirements:
        /// 1. Should not create account if email is in use.
        /// 2. Should use repository to get new user model.
        /// 3. Should call Create on repository.
        /// 4. Should call Save on repository.
        /// 5. Should assign email, first name, and last name to new user model.
        /// 6. Should setup new credentials.
        /// </summary>
        public UserAccount CreateAccount(UserAccount newAccount, Credentials newCredentials)
        {
            VerifyEmailIsntInUse(newAccount);

            var newUser = _userCreator.New();
            newUser.Email = newAccount.Email;
            newUser.FirstName = newAccount.FirstName;
            newUser.LastName = newAccount.LastName;

            _userCreator.Create(newUser);
            _userCreator.Save();

            var userAccount = _translateDataUserToUserAccount.Translate(newUser);

            _userAuthProvider.SetupNewCredentials(userAccount, newCredentials);

            return userAccount;
        }

        public UserAccount RetrieveAccount(Credentials existingCredentials)
        {
            var authenticationResult = _userAuthProvider.Authenticate(existingCredentials);
            if (authenticationResult.WasSuccessful)
            {
                UpdateLastLogin(authenticationResult.UserAccount.UserId);
                return authenticationResult.UserAccount;
            }
            return null;
        }

        public UserAccount UpdateAccount(UserAccount existingAccount, Credentials existCredentials)
        {
            var query = new ByUserId
            {
                UserId = existingAccount.UserId
            };

            var updatedUser = _userGetByUserId.Retrieve(query);

            updatedUser.Email = existingAccount.Email;
            updatedUser.FirstName = existingAccount.FirstName;
            updatedUser.LastName = existingAccount.LastName;

            _userUpdater.Update(updatedUser);
            _userUpdater.Save();

            return _translateDataUserToUserAccount.Translate(updatedUser);
        }

        public void DeleteAccount(UserAccount existingAccount, Credentials existCredentials)
        {
            var authenticationResult = _userAuthProvider.Authenticate(existCredentials);
            if (authenticationResult.WasSuccessful)
            {
                _userAuthProvider.DeleteCredentials(existCredentials);
                
                var query = new ByUserId
                {
                    UserId = existingAccount.UserId
                };

                var existingUser = _userGetByUserId.Retrieve(query);
                _userDeleter.Delete(existingUser);
            }
        }

        /// <summary>
        /// Buisness Requirements:
        /// 1. Verify if Email address is in use.
        /// 2. If it is in use, throw exception.
        /// 3. Exception message should include email that was found to be in use.
        /// 4. If not in use, do nothing.
        /// </summary>
        public void VerifyEmailIsntInUse(UserAccount newAccount)
        {
            var query = new ByUserEmail
            {
                UserEmail = newAccount.Email
            };

            var userDetails = _retrieveUserByEmail.Retrieve(query);

            if (userDetails == null) throw new ArgumentException("Email Address: {0} already in use.", newAccount.Email);
        }

        public void UpdateLastLogin(string userId)
        {
            var query = new ByUserId
            {
                UserId = userId
            };

            var updatedUser = _userGetByUserId.Retrieve(query);

            updatedUser.LastLogin = _systemTime.Current();

            _userUpdater.Update(updatedUser);
            _userUpdater.Save();
        }
    }
}
