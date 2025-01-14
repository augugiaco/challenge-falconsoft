namespace FalconSoftChallenge.DAL.DTO
{
    public class PagedResultDTO<C> where C : class
    {
        public List<C> Data { get; init; }

        public int TotalItems { get; init; }

        public int Page { get; init; }

        public int PageSize { get; init; }

        public int TotalPagesCount { get; init; }

        public PagedResultDTO(List<C> data, int totalItems, int page, int pageSize, int totalPagesCount)
        {
            Data = data;
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
            TotalPagesCount = totalPagesCount;
        }
    }
}
