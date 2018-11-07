using SurveyOnline.Constants;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SurveyOnline.Web.Helper
{
    public class SurveyOnlineHttpClient
    {
        public static HttpClient GetHttpClient(string accessToken)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(SurveyOnlineConstants.SurveyOnlineWepApi)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return client;
        }

        public static HttpClient GetHttpClient()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(SurveyOnlineConstants.SurveyOnlineWepApi)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
