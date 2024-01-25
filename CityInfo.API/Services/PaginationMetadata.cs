namespace CityInfo.API.Services
{
    public class PaginationMetadata
    {
        public int TotalIntemsCount { get; set; }
        public int TotalPageCount { get; set; }
        public int PageSize { get; set; }
        public int CurrenPage {  get; set; }

        public PaginationMetadata(int totalItemCount, int pageSize,  int currenPage)
        {
            TotalIntemsCount = totalItemCount;
            PageSize = pageSize;
            CurrenPage = currenPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }

    }
}
