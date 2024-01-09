using BlogApi2.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BlogApi2.Data
{
    public class BlogContext : IBlogContext
    {
        public BlogContext(IConfiguration configuration)
        {
            //var client = new MongoClient(settings.ConnectionString);
            //var database = client.GetDatabase(settings.DatabaseName);
            //Blogs = database.GetCollection<Blog>(settings.CollectionName);
            //BlogContextSeed.SeedData(Blogs);

            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Blogs = database.GetCollection<Blog>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            BlogContextSeed.SeedData(Blogs);
        }
        public IMongoCollection<Blog> Blogs { get; }
    }
}
