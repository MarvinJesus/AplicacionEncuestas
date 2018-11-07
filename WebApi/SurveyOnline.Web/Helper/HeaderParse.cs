using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;

namespace SurveyOnline.Web.Helper
{
    public static class HeaderParse
    {
        public static PagingInfo FindAndParsePagingInfo(HttpResponseHeaders responseHeader)
        {
            if (responseHeader.Contains("X-Pagination"))
            {
                var xPag = responseHeader.First(xP => xP.Key == "X-Pagination").Value;

                return JsonConvert.DeserializeObject<PagingInfo>(xPag.First());
            }

            return null;
        }
    }
}