using Entities_POJO;
using Newtonsoft.Json;
using SurveyOnline.Web.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Web.Services
{
    public class SurveyService : Service
    {
        private readonly string SURVEY_CONTROLLER_NAME = "surveys";
        private readonly string TOPIC_CONTROLLER_NAME = "topics";

        public SurveyService(string accessToken) : base(accessToken)
        {
        }

        public async Task<Survey> RegisterSurveyAsync(Survey survey)
        {
            if (survey == null) return null;

            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            Survey surveyRegistered = null;

            var result = await client.PostAsync(
                string.Format($"api/{TOPIC_CONTROLLER_NAME}/{survey.TopicId}/{SURVEY_CONTROLLER_NAME}"),
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(survey),
                Encoding.Unicode, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                surveyRegistered = Newtonsoft.Json.JsonConvert.DeserializeObject<Survey>(content);
            }

            return surveyRegistered;
        }

        public async Task<ICollection<Survey>> GetTopiSurveys(Guid topicId)
        {
            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            ICollection<Survey> surveys = null;

            var result = await client.GetAsync($"api/{TOPIC_CONTROLLER_NAME}/{topicId.ToString()}/{SURVEY_CONTROLLER_NAME}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                surveys = JsonConvert.DeserializeObject<ICollection<Survey>>(content);
            }

            return surveys;
        }
    }
}