namespace Application.DTOs
{
    public class PaginationRequestDTO
    {
        private int _page = 1;
        private int _pageSize = 10;

        public int Page
        {
            get => _page;
            set => _page = (value <= 0) ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value <= 0) ? 10 : (value > 100 ? 100 : value); // Limitando máximo 100 registros
        }

        public string? Search { get; set; }
        public string? OrderBy { get; set; }
        public bool Ascending { get; set; } = true;
    }
}
