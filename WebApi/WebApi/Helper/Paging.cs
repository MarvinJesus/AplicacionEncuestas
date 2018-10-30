using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Http.Routing;

namespace WebApi.Helper
{
    public class Paging
    {
        private static Paging _paging { get; set; }

        private Paging()
        {
        }

        public static Paging GetInstance()
        {
            if (_paging == null) _paging = new Paging();

            return _paging;
        }

        public string Page(int page, int pageSize, int maxPageSize, IQueryable source, string routeName, UrlHelper urlHelper)
        {
            return Page(page, pageSize, maxPageSize, source, routeName, urlHelper, "", "", "", "");
        }

        public string Page(int page, int pageSize, int maxPageSize, IQueryable source, string routeName,
             UrlHelper urlHelper, string sort = "", string fields = "")
        {
            return Page(page, pageSize, maxPageSize, source, routeName, urlHelper, sort, "", fields, "");
        }

        public string Page(int page, int pageSize, int maxPageSize, IQueryable source, string routeName,
         UrlHelper urlHelper, string sort = "", string filters = "", string fields = "", string search = "")
        {
            if (source == null) throw new ArgumentNullException("source");

            if (urlHelper == null) throw new ArgumentNullException("urlHelper");

            if (pageSize > maxPageSize) pageSize = maxPageSize;

            var totalCount = source.Count();
            var totalPages = TotalPage(totalCount, pageSize);

            return PaginationHeader(page, pageSize, totalCount, totalPages,
                PrevLink(page, urlHelper, routeName, pageSize, sort, filters, fields, search),
                NextLink(page, totalPages, urlHelper, routeName, pageSize, sort, filters, fields, search));
        }

        private int TotalPage(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling((double)totalCount / pageSize);
        }

        private string PaginationHeader(int currentPage, int pageSize, int totalCount, int totalPages, string prevLink, string nextLink)
        {
            var paginationHeader = new
            {
                currentPage,
                pageSize,
                totalCount,
                totalPages,
                previousLink = prevLink,
                nextLink
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader);
        }

        private string NextLink(int page, int totalPages, UrlHelper urlHelper, string routeName,
            int pageSize, string sort, string filters, string fields, string search)
        {
            var nextLink = page < totalPages ? urlHelper.Link(routeName, new
            {
                page = page + 1,
                pageSize,
                sort,
                filters,
                fields,
                search
            }) : "";

            return nextLink;
        }

        private string PrevLink(int page, UrlHelper urlHelper, string routeName,
            int pageSize, string sort, string filters, string fields, string search)
        {
            var prevLink = page > 1 ? urlHelper.Link(routeName, new
            {
                page = page - 1,
                pageSize,
                sort,
                filters,
                fields,
                search
            }) : "";

            return prevLink;
        }
    }
}