using BlogApi2.Entities;
using MongoDB.Driver;

namespace BlogApi2.Data
{
    public interface IBlogContext
    {
        IMongoCollection<Blog> Blogs { get; }
    }
}
