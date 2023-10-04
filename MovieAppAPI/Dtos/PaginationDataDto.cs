namespace MovieAppAPI.Dtos
{
    public class PaginationDataDto
    {
        public int ItemsCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPagesCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}
