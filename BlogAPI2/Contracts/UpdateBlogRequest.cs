using System.ComponentModel.DataAnnotations;

namespace BlogAPI2.Contracts;

public class UpdateBlogRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }
}