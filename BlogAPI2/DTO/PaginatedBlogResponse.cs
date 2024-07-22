namespace BlogAPI2.DTO
{
    public class PaginatedBlogResponse
    {
        public int Count { get; set; }

        public List<BlogResponseDto> Blogs { get; set; }
    }
}
