namespace BillyGoats.Api.Filter
{
    public class Sort
    {
        public string Field { get; set; }

        public SortDirection Dir { get; set; } = SortDirection.Asc;
    }
}
