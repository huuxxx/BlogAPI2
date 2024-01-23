using BlogApi2.Entities;
using BlogApi2.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BlogApi2.Services
{
    public class BlogService
    {
        private readonly IMongoCollection<Blog> _blogsCollection;

        public BlogService(
            IOptions<BlogDatabaseSettings> blogDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                blogDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                blogDatabaseSettings.Value.DatabaseName);

            _blogsCollection = mongoDatabase.GetCollection<Blog>(
                blogDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Blog>> GetAsync() =>
            await _blogsCollection.Find(_ => true).ToListAsync();

        public async Task<Blog?> GetAsync(string id) =>
            await _blogsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Blog newBlog) =>
            await _blogsCollection.InsertOneAsync(newBlog);

        public async Task UpdateAsync(string id, Blog updatedBlog) =>
            await _blogsCollection.ReplaceOneAsync(x => x.Id == id, updatedBlog);

        public async Task RemoveAsync(string id) =>
            await _blogsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
