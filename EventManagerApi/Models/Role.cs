namespace EventManagerApi.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Навігація на користувачів цієї ролі
        public List<User> Users { get; set; } = new();
    }
}
