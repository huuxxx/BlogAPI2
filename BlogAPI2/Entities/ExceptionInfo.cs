using System.ComponentModel.DataAnnotations;

namespace BlogAPI2.Entities
{
    public class ExceptionInfo
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
    }
}
