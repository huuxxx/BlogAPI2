﻿namespace BlogAPI2.DTO
{
    public class BlogResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int ViewCount { get; set; }
        public List<string> Tags { get; set; }
    }
}
