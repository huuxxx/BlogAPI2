using System.ComponentModel.DataAnnotations;

namespace BlogAPI2.Contracts
{
    public class UpdateVisitorRequest
    {
        [Required]
        public Guid Id { get; set; }

        public bool ViewedBlogs { get; set; }

        public bool ViewedProjects { get; set; }

        public bool ViewedAbout { get; set; }

        public bool ViewedSearch { get; set; }
    }
}
