using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SMG.Core.Data.MongoDB.Attributes;

namespace UnitTestWorkshop.Data.Models.AccountModels
{
    /// <summary>
    /// Business Requirements:
    /// 1. Must obscure meaning of fields
    /// 2. Model must map to obscured collection name of additionalinfo
    /// 3. Model must serialize down to BSON document with the following properties
    /// _id
    /// key
    /// value
    /// fac
    /// active
    /// at
    /// </summary>
    [BsonCollectionName("additionalinfo")]
    public class UserAuthentication
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string EncryptionKey { get; set; }

        [BsonElement("key")]
        public string EncodedUserId { get; set; }

        [BsonElement("value")]
        public string EncodedPassword { get; set; }

        [BsonElement("fac")]
        public int FailedLoginAttemptCount { get; set; }

        [BsonElement("active")]
        public bool AccountActive { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        [BsonElement("at")]
        public AccountType AccountType { get; set; }
    }
}
