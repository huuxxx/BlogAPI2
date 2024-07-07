namespace BlogAPI2.Contracts
{
    public class UpdateVisitorRequest
    {
        public Guid Id { get; set; }

        public bool ViewedBlogs { get; set; }

        public bool ViewedProjects { get; set; }

        public bool ViewedAbout { get; set; }
    }
}
