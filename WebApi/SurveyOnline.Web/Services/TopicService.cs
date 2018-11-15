using Entities_POJO;
using Newtonsoft.Json;
using SurveyOnline.Web.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SurveyOnline.Web.Services
{
    public class TopicService : Service
    {
        private readonly string TOPIC_CONTROLLER_NAME = "topics";
        private readonly string PROFILE_CONTROLLER_NAME = "profiles";
        private HttpResponseHeaders ResponseHeaders { get; set; }

        public TopicService(string accessToken) : base(accessToken)
        {
        }

        public PagingInfo GetPagingInfo()
        {
            return HeaderParse.FindAndParsePagingInfo(ResponseHeaders);
        }

        public async Task<ICollection<Topic>> GetUserTopicsAsync(Guid userId, Dictionary<string, string> param)
        {
            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            ICollection<Topic> topics = null;

            var result = await client.GetAsync($"api/{PROFILE_CONTROLLER_NAME}/{userId}/{TOPIC_CONTROLLER_NAME}?{InsertParams(param)}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                topics = JsonConvert.DeserializeObject<ICollection<Topic>>(content);
            }

            return topics;
        }
        public async Task<ICollection<Topic>> GetTopicsAsync(Dictionary<string, string> param)
        {
            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            ICollection<Topic> topics = null;

            var result = await client.GetAsync(string.Format($"api/{TOPIC_CONTROLLER_NAME}?{InsertParams(param)}"));

            if (result.IsSuccessStatusCode)
            {
                var topicsString = await result.Content.ReadAsStringAsync();

                topics = Newtonsoft.Json.JsonConvert.DeserializeObject<ICollection<Topic>>(topicsString);

                ResponseHeaders = result.Headers;
            }

            return topics;
        }

        private string InsertParams(Dictionary<string, string> paramsForRequest)
        {
            var paramList = string.Empty;
            var first = true;

            if (paramsForRequest == null) return paramList;

            foreach (var param in paramsForRequest)
            {
                if (first)
                {
                    paramList = string.Format($"{param.Key}={param.Value}");
                    first = false;
                }
                else
                {
                    paramList += string.Format($"&{param.Key}={param.Value}");
                }
            }

            return paramList;
        }

        private string InsertNewParam(string type, string value, string paramToInsertValue)
        {
            if (!string.IsNullOrEmpty(value))
            {
                paramToInsertValue += string.Format($"{type}={value}&");
            }

            return paramToInsertValue;
        }
    }
}