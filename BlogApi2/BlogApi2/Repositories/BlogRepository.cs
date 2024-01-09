using BlogApi2.Data;
using BlogApi2.Entities;
using MongoDB.Driver;

namespace BlogApi2.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IBlogContext _blogContext;

        public BlogRepository(IBlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public async Task CreateBlog(Blog blog)
        {
            await _blogContext.Blogs.InsertOneAsync(blog);
        }

        public async Task<bool> DeleteBlog(Guid id)
        {
            FilterDefinition<Blog> filter = Builders<Blog>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _blogContext
                                                .Blogs
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<Blog> GetBlog(Guid id)
        {
            return await _blogContext
                           .Blogs
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogByTitle(string name)
        {
            FilterDefinition<Blog> filter = Builders<Blog>.Filter.ElemMatch(p => p.Title, name);

            return await _blogContext
                            .Blogs
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await _blogContext
                            .Blogs
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<bool> UpdateBlog(Blog blog)
        {
            var updateResult = await _blogContext
                            .Blogs
                            .ReplaceOneAsync(filter: g => g.Id == blog.Id, replacement: blog);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
