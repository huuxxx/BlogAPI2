namespace BlogAPI2.Entities
{
    public class BlogTag
    {
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
