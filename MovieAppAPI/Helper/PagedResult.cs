namespace MovieAppAPI.Helper
{
    public class PagedResult<T>
    {
        public int ItemsCount { get; private set; }
        public int PageSize { get; private set; }
        public int CurrentPageNumber { get; private set; }
        public int TotalPagesCount { get; private set; }
        public bool HasPrevious => CurrentPageNumber > 1;
        public bool HasNext => CurrentPageNumber < TotalPagesCount;
        public ICollection<T> items { get; private set; }

        public PagedResult(IQueryable<T> items, int pageNumber, int pageSize)
        {
            ItemsCount = items.Count();
            PageSize = pageSize;
            CurrentPageNumber = pageNumber;
            TotalPagesCount = (int)Math.Ceiling(ItemsCount / (double)pageSize);
            this.items = items.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
        }
    }
}
