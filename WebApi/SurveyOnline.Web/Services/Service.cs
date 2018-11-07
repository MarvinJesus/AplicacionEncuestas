namespace SurveyOnline.Web.Services
{
    public class Service
    {
        protected string _accessToken { get; set; }

        public Service(string accessToken)
        {
            _accessToken = accessToken;
        }
    }
}