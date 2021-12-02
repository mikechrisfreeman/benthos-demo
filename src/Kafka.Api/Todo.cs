namespace Kafka.Api
{
    public class Todo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? EnrichmentText { get; set; }
        public bool Done { get; set; }
    }
}
