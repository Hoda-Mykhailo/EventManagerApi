namespace EventManagerApi.Models.DTO
{
    public class EventDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}
