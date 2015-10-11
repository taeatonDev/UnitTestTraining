using System;
using System.Collections.Generic;
using System.Linq;
using SMG.Core.Contracts;
using SMG.Core.Data.Contracts;
using UnitTestWorkshop.Business.Handlers;
using UnitTestWorkshop.Business.Models.AccountModels;
using UnitTestWorkshop.Data.Models.AccountModels;
using UnitTestWorkshop.Data.Models.QueryModels;

namespace UnitTestWorkshop.Business.Providers
{
    /// <summary>
    /// Business Requirements:
    /// 1. Must provide CRUD functionality for 'False Door' Authentication Strategy
    /// 2. Each user must have 4 stored authentication records
    /// 2.a One of those records is the real authentication
    /// 2.b The other records are fake
    /// 3. If user successfully logs into fake record, Lock the entire account.
    /// 4. If user successfully logs into real record, reset failed tries to 0.
    /// 5. If user fails to log in, increment failed tries.
    /// 6. If user fails to log in more than 3 times, Lock the entire account.
    /// 7. UserId Must be encrypted
    /// 7.a UserId salt should be the UserId
    /// 8. Password Must be encrypted
    /// 8.a Passsword salt should be the EncryptionKey for the record 
    /// 9. If successful return UserAccount.
    /// </summary>
    public class UserAuthenticationProvider : IUserAuthenticationProvider
    {
        private readonly ITranslate<User, UserAccount> _translateDataUserToUserAccount;
        private readonly IRetrievable<ByUserEmail, User> _retrieveUserByEmail;
        private readonly IBulkRetrievable<ByEncodedUserId, UserAuthentication> _retrieveAllUserAuthenticationByEncodedUserId;
        private readonly IUpdatable<UserAuthentication> _userAuthUpdater;
        private readonly ICreatable<UserAuthentication> _userAuthCreator;
        private readonly IPasswords _passwords;
        private readonly IDeletable<UserAuthentication> _userAuthDeleter;

        public UserAuthenticationProvider(
            ITranslate<User, UserAccount> translateDataUserToUserAccount, 
            IRetrievable<ByUserEmail, User> retrieveUserByEmail, 
            IBulkRetrievable<ByEncodedUserId, UserAuthentication> retrieveAllUserAuthenticationByEncodedUserId, 
            IUpdatable<UserAuthentication> userAuthUpdater, 
            ICreatable<UserAuthentication> userAuthCreator, 
            IPasswords passwords, 
            IDeletable<UserAuthentication> userAuthDeleter)
        {
            _translateDataUserToUserAccount = translateDataUserToUserAccount;
            _retrieveUserByEmail = retrieveUserByEmail;
            _retrieveAllUserAuthenticationByEncodedUserId = retrieveAllUserAuthenticationByEncodedUserId;
            _userAuthUpdater = userAuthUpdater;
            _userAuthCreator = userAuthCreator;
            _passwords = passwords;
            _userAuthDeleter = userAuthDeleter;
        }

        private const int MaxAttemptsAllowed = 6;

        public AuthenticationResult Authenticate(Credentials credentials)
        {
            try
            {
                var user = GetUser(credentials.Email);
                var authCollection = GetUserAuthCollection(user);
                var validateResult = ValidateCredentials(authCollection, credentials.Password);

                switch (validateResult)
                {
                    case ValidateResult.Valid:
                        ResetLoginAttempts(authCollection);
                        return new AuthenticationResult
                        {
                            WasSuccessful = true,
                            UserAccount = _translateDataUserToUserAccount.Translate(user)
                        };
                    case ValidateResult.Invalid:
                        ProcessFailedAttempt(authCollection);
                        break;
                    case ValidateResult.LockAccount:
                        LockAccount(authCollection);
                        break;
                }
            }
            catch (Exception ex)
            {
                //TODO: Do Logging
            }

            return new AuthenticationResult
            {
                WasSuccessful = false
            };

            
        }

        public AuthenticationResult ChangePassword(UpdateCredentials credentials)
        {
            try
            {
                var user = GetUser(credentials.Email);
                var authCollection = GetUserAuthCollection(user);
                var validateResult = ValidateCredentials(authCollection, credentials.Password);

                if (validateResult == ValidateResult.Valid)
                {
                    UpdateUserCredentials(authCollection, credentials);
                    return new AuthenticationResult
                    {
                        WasSuccessful = true,
                        UserAccount = _translateDataUserToUserAccount.Translate(user)
                    };
                }
            }
            catch (Exception ex)
            {
                //TODO: Do Logging
            }

            return new AuthenticationResult
            {
                WasSuccessful = false
            };
        }

        public void SetupNewCredentials(UserAccount userAccount, Credentials credentials)
        {
            string encodedUserId = EncryptionHandler.Encrypt(userAccount.UserId, userAccount.UserId);

            var actualCredentials = _userAuthCreator.New();
            actualCredentials.AccountType = AccountType.ActualAccount;
            actualCredentials.EncodedUserId = encodedUserId;
            actualCredentials.EncodedPassword = EncryptionHandler.Encrypt(credentials.Password, actualCredentials.EncryptionKey);
            _userAuthCreator.Create(actualCredentials);

            var trap1Credentials = _userAuthCreator.New();
            trap1Credentials.AccountType = AccountType.Trap1Account;
            trap1Credentials.EncodedUserId = encodedUserId;
            trap1Credentials.EncodedPassword = EncryptionHandler.Encrypt(_passwords.GeneratePassword(8,2), trap1Credentials.EncryptionKey);
            _userAuthCreator.Create(trap1Credentials);

            var trap2Credentials = _userAuthCreator.New();
            trap2Credentials.AccountType = AccountType.Trap2Account;
            trap2Credentials.EncodedUserId = encodedUserId;
            trap2Credentials.EncodedPassword = EncryptionHandler.Encrypt(_passwords.GeneratePassword(10, 3), trap2Credentials.EncryptionKey);
            _userAuthCreator.Create(trap2Credentials);

            var trap3Credentials = _userAuthCreator.New();
            trap3Credentials.AccountType = AccountType.Trap3Account;
            trap3Credentials.EncodedUserId = encodedUserId;
            trap3Credentials.EncodedPassword = EncryptionHandler.Encrypt(_passwords.GeneratePassword(7, 1), trap3Credentials.EncryptionKey);
            _userAuthCreator.Create(trap3Credentials);
        }

        public void DeleteCredentials(Credentials credentials)
        {
            try
            {
                var user = GetUser(credentials.Email);
                var authCollection = GetUserAuthCollection(user);
                var validateResult = ValidateCredentials(authCollection, credentials.Password);

                if (validateResult == ValidateResult.Valid)
                {
                    foreach (var authCredential in authCollection)
                    {
                        _userAuthDeleter.Delete(authCredential);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Do Logging
            }
        }

        public void UpdateUserCredentials(IList<UserAuthentication> authCollection, UpdateCredentials credentials)
        {
            var actualAccount = authCollection.First(x => x.AccountType == AccountType.ActualAccount);
            var newPassword = EncryptionHandler.Encrypt(credentials.NewPassword, actualAccount.EncryptionKey);
            actualAccount.EncodedPassword = newPassword;
            _userAuthUpdater.Update(actualAccount);
        }

        public void LockAccount(IList<UserAuthentication> authCollection)
        {
            foreach (var userAuthentication in authCollection)
            {
                userAuthentication.AccountActive = false;
                _userAuthUpdater.Update(userAuthentication);
            }
        }

        public ValidateResult ValidateCredentials(IList<UserAuthentication> authCollection, string password)
        {
            var passwordIsValid = ValidateResult.Invalid;

            foreach (var userAuthentication in authCollection)
            {
                var encodedPassword = EncryptionHandler.Encrypt(password, userAuthentication.EncryptionKey);
                if (
                    string.Compare(userAuthentication.EncodedPassword, encodedPassword,
                        StringComparison.InvariantCultureIgnoreCase) == 1)
                {
                    passwordIsValid = userAuthentication.AccountType == AccountType.ActualAccount ? 
                        ValidateResult.Valid : 
                        ValidateResult.LockAccount;
                }
            }

            return passwordIsValid;
        }

        public void ProcessFailedAttempt(IList<UserAuthentication> authCollection)
        {
            var currentAttemptCount = authCollection.First().FailedLoginAttemptCount;

            var maxAttemptsReached = currentAttemptCount + 1 >= MaxAttemptsAllowed;

            foreach (var userAuthentication in authCollection)
            {
                userAuthentication.FailedLoginAttemptCount ++;
                if (maxAttemptsReached) userAuthentication.AccountActive = false;
                _userAuthUpdater.Update(userAuthentication);
            }
        }

        public void ResetLoginAttempts(IList<UserAuthentication> authCollection)
        {
            foreach (var userAuthentication in authCollection)
            {
                userAuthentication.FailedLoginAttemptCount = 0;
                _userAuthUpdater.Update(userAuthentication);
            }
        }

        public IList<UserAuthentication> GetUserAuthCollection(User user)
        {
            var encodedUserId = EncryptionHandler.Encrypt(user.UserId, user.UserId);
            var query = new ByEncodedUserId
            {
                EncodedUserId = encodedUserId
            };

            return _retrieveAllUserAuthenticationByEncodedUserId.RetrieveAll(query);
        }

        public User GetUser(string email)
        {
            var query = new ByUserEmail
            {
                UserEmail = email
            };

            var userDetails = _retrieveUserByEmail.Retrieve(query);

            if(userDetails == null) throw new ArgumentException("Email Address: {0} has no matching account.", email);
            
            return userDetails;
        }
    }
}
