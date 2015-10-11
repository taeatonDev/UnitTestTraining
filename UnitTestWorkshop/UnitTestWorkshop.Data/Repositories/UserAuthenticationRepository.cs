using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using SMG.Core.Data.Contracts;
using SMG.Core.Data.MongoDB;
using UnitTestWorkshop.Data.Databases;
using UnitTestWorkshop.Data.Models.AccountModels;
using UnitTestWorkshop.Data.Models.QueryModels;

namespace UnitTestWorkshop.Data.Repositories
{
    /// <summary>
    /// Business Requirements:
    /// 1. Provide a Access to the data stored in the database using a strongly typed model.
    /// 2. Prevent consuming code from knowing what storage engine is being used.
    /// </summary>
    public class UserAuthenticationRepository : BaseMongoRepository<UnitTestDatabase,UserAuthentication>,
        ICreatable<UserAuthentication>,
        IUpdatable<UserAuthentication>,
        IDeletable<UserAuthentication>,
        IBulkRetrievable<ByEncodedUserId, UserAuthentication>
    {
        /// <summary>
        /// Business Requirements:
        /// 1. Provide a new model with defaulted values.
        /// 2. Encryption key must be provided when new model is requested
        /// </summary>
        /// <returns>New User Authentication Model</returns>
        public UserAuthentication New()
        {
            return new UserAuthentication
            {
                EncryptionKey = new ObjectId().ToString(),
                FailedLoginAttemptCount = 0,
                AccountActive = true,
                EncodedPassword = string.Empty,
                EncodedUserId = string.Empty
            };
        }

        public IList<UserAuthentication> RetrieveAll(ByEncodedUserId input)
        {
            var db = base.GetMongoDatabase();
            var collection = db.GetCollection<UserAuthentication>(CollectionName);

            var documents = collection
                                .AsQueryable()
                                .Where(x => x.EncodedUserId == input.EncodedUserId
                                && x.AccountActive)
                                .ToList();

            return documents;
        }
    }
}
