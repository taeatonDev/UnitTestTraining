using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SMG.Core.Data.MongoDB.Attributes;

namespace UnitTestWorkshop.Data.Models.AccountModels
{
    /// <summary>
    /// Business Requirements:
    /// 1. Must Serialize down to BSON Document with the following properties:
    /// _id
    /// emailaddress
    /// fname
    /// lname
    /// lastlogindate
    /// creationdate
    /// </summary>
    [BsonCollectionName("users")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string UserId { get; set; }

        [BsonElement("emailaddress")]
        public string Email { get; set; }

        [BsonElement("fname")]
        public string FirstName { get; set; }

        [BsonElement("lname")]
        public string LastName { get; set; }

        [BsonElement("lastlogindate")]
        public DateTime LastLogin { get; set; }

        [BsonElement("creationdate")]
        public DateTime CreationDate { get; set; }
    }
}
