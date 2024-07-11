namespace BlogAPI2.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
