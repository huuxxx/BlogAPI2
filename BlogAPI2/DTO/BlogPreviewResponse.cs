namespace BlogAPI2.DTO
{
    public class BlogPreviewResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public int ViewCount { get; set; }
    }
}
