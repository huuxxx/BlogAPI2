using System.ComponentModel.DataAnnotations;

namespace BlogAPI2.Contracts;

public class CreateBlogRequest
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public string[] Tags { get; set; }
}