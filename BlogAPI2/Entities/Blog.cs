﻿using System.ComponentModel.DataAnnotations;

namespace BlogAPI2.Entities
{
    public class Blog
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int ViewCount { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
