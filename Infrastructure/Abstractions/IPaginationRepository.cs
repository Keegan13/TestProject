namespace Infrastructure.Abstractions
{
    public interface IPaginationRepository
    {
        int LastQueryTotalCount { get; }
    }
}
