namespace MovieAppAPI.Helper
{
    public class QueryStringParameters
    {
        private int maxPageSize = 50;
        private int pageSize = 20;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string? OrderBy { get; set; }

    }
}
