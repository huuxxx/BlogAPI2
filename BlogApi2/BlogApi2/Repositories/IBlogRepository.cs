using BlogApi2.Entities;

namespace BlogApi2.Repositories
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetBlogs();
        Task<Blog> GetBlog (Guid id);
        Task<IEnumerable<Blog>> GetBlogByTitle(string name);
        Task CreateBlog(Blog blog);
        Task<bool> UpdateBlog(Blog blog);
        Task<bool> DeleteBlog(Guid id);
    }
}
