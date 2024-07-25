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
                Tags = blog.BlogTags.OrderBy(x => x.Tag.Name).Select(bt => bt.Tag.Name).ToList()
            };
        }

        public static List<BlogResponseDto> BlogEntityToDto(List<Blog> blogs)
        {
            return blogs.Select(blog => BlogEntityToDto(blog)).ToList();
        }

        public static BlogPreviewResponse BlogEntityToPreview(Blog blog)
        {
            return new BlogPreviewResponse
            {
                Id = blog.Id,
                Title = blog.Title,
                DateCreated = blog.DateCreated,
                ViewCount = blog.ViewCount,
            };
        }

        public static List<BlogPreviewResponse> BlogEntityToPreview(List<Blog> blogs)
        {
            return blogs.Select(blog => BlogEntityToPreview(blog)).ToList();
        }
    }
}
