using System.ComponentModel.DataAnnotations;

namespace EventManagerApi.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
