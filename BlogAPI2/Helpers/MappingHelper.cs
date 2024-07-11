using BlogAPI2.DTO;
using BlogAPI2.Entities;

namespace BlogAPI2.Helpers
{
    public static class MappingHelper
    {
        public static BlogResponseDto BlogEntityToDto (Blog blog)
        {
            return new BlogResponseDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                DateCreated = blog.DateCreated,
                DateModified = blog.DateModified,
                ViewCount = blog.ViewCount,
                Tags = blog.BlogTags.Select(bt => bt.Tag.Name).ToList()
            };
        }

        public static List<BlogResponseDto> BlogEntityToDto(List<Blog> blogs)
        {
            return blogs.Select(blog => BlogEntityToDto(blog)).ToList();
        }
    }
}
