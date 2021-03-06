﻿using Entities_POJO;
using Newtonsoft.Json;
using SurveyOnline.Web.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        public async Task<ICollection<Topic>> GetTopicsAsync(Dictionary<string, string> param = null)
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

        public async Task<Topic> GetTopicAsync(Guid id, Dictionary<string, string> param)
        {
            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            Topic topic = null;

            var result = await client.GetAsync($"api/{TOPIC_CONTROLLER_NAME}/{id.ToString()}?{InsertParams(param)}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                topic = JsonConvert.DeserializeObject<Topic>(content);
            }

            return topic;
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

        public async Task<Topic> RegisterTopicAsync(Guid userId, TopicForRegistration topicForRegistration)
        {
            if (topicForRegistration == null) return null;

            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            var objectSerialized = JsonConvert.SerializeObject(topicForRegistration);
            Topic topic = null;

            var result = await client.PostAsync($"api/{PROFILE_CONTROLLER_NAME}/{userId.ToString()}/{TOPIC_CONTROLLER_NAME}",
                new StringContent(objectSerialized, Encoding.Unicode, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                topic = JsonConvert.DeserializeObject<Topic>(content);
            }

            return topic;
        }
    }
}