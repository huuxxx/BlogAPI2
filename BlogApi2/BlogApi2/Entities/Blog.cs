using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlogApi2.Entities
{
    public class Blog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastEdit { get; set; }

        public int ViewCount { get; set; }
    }
}
