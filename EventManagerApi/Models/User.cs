using System.ComponentModel.DataAnnotations;

namespace EventManagerApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Замість зберігання рядка, зберігаємо зовнішній ключ на Role
        public int RoleId { get; set; }

        // Навігаційна властивість
        public Role? Role { get; set; }

        public List<Event> Events { get; set; } = new();
    }
}
