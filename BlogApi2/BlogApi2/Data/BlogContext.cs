using BlogApi2.Entities;
using MongoDB.Driver;

namespace BlogApi2.Data
{
    public class BlogContext : IBlogContext
    {
        public BlogContext(IConfiguration configuration)
        {
            //var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            //var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            //Blogs = database.GetCollection<Blog>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        }
        public IMongoCollection<Blog> Blogs { get; }
    }
}
