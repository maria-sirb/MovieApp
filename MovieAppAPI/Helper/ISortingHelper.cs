namespace MovieAppAPI.Helper
{
    public interface ISortingHelper<T> where T : class
    {
        IQueryable<T> ApplySort(IQueryable<T> entities, string orderByParameters);
    }
}
