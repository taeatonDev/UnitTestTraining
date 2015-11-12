using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using SMG.Core;
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
    public class UserRepository : BaseMongoRepository<UnitTestDatabase, User>,
        ICreatable<User>,
        IUpdatable<User>,
        IDeletable<User>,
        IRetrievable<ByUserEmail,User>,
        IRetrievable<ByUserId, User>
    {
        private readonly ISystemTime _systemTime;

        public UserRepository(ISystemTime systemTime)
        {
            _systemTime = systemTime;
        }

        /// <summary>
        /// Business Requirements:
        /// 1. Provide a new model with defaulted initial values.
        /// 2. UserId must be generated and provided on new model creation.
        /// 3. Create Date must be set to current Date Time in UTC on new model creation.
        /// </summary>
        /// <returns></returns>
        public User New()
        {
            var creationDate = _systemTime.Current();

            return new User
            {
                UserId = new ObjectId().ToString(),
                CreationDate = creationDate,
                Email = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                LastLogin = creationDate
            };
        }

        public User Retrieve(ByUserEmail input)
        {
            var db = base.GetMongoDatabase();
            var collection = db.GetCollection<User>(CollectionName);

            var document = collection
                                .AsQueryable()
                                .FirstOrDefault(x => x.Email == input.UserEmail);

            return document;
        }

        public User Retrieve(ByUserId input)
        {
            var db = base.GetMongoDatabase();
            var collection = db.GetCollection<User>(CollectionName);

            var document = collection
                                .AsQueryable()
                                .FirstOrDefault(x => x.UserId == input.UserId);

            return document;
        }
    }
}
