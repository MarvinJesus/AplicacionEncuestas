namespace SurveyOnline.Web.Helper
{
    public class PagingInfo
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string PreviousLik { get; set; }
        public string NextLink { get; set; }

        public PagingInfo(int totalCount, int totalPage, int currentPage, int pageSize, string previousLink, string nextLink)
        {
            TotalCount = totalCount;
            TotalPage = totalPage;
            CurrentPage = currentPage;
            PageSize = pageSize;
            PreviousLik = previousLink;
            NextLink = nextLink;
        }
    }
}