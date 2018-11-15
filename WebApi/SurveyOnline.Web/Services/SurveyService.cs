using Entities_POJO;
using SurveyOnline.Web.Helper;
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
    }
}