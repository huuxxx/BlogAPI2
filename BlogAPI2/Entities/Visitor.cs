namespace BlogAPI2.Entities
{
    public class Visitor
    {
        public Guid Id { get; set; }
        public string VisitorIp { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public DateTime DateVisited { get; set; }
        public bool ViewedBlogs { get; set; }
        public bool ViewedProjects { get; set; }
        public bool ViewedAbout { get; set; }
    }
}
