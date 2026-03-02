namespace vita_care.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public long TotalCount { get; set; }
    }
}
