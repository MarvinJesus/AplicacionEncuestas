using Entities_POJO;
using Newtonsoft.Json;
using SurveyOnline.Constants;
using SurveyOnline.Web.Helper;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Web.Services
{
    public class ProfileService : Service
    {
        private readonly string PROFILE_CONTROLLER = "profiles";

        public ProfileService(string accessToken) : base(accessToken)
        {
        }

        public async Task<Profile> GetProfileAsync(Guid profileId)
        {
            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            Profile profile = null;

            var result = await client.GetAsync(string.Format($"api/{PROFILE_CONTROLLER}?userId={profileId.ToString()}"));

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                profile = JsonConvert.DeserializeObject<Profile>(content);
            }

            return profile;
        }

        public Profile GetProfile(Guid profileId)
        {
            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            Profile profile = null;

            var result = client.GetAsync(string.Format($"api/{PROFILE_CONTROLLER}?userId={profileId.ToString()}"));
            Task.WhenAll(result);

            if (result.Result.IsSuccessStatusCode)
            {
                var content = result.Result.Content.ReadAsStringAsync();
                Task.WhenAll(content);

                profile = JsonConvert.DeserializeObject<Profile>(content.Result);
            }

            return profile;
        }

        public async Task<Profile> ChangePictureAsync(Stream file, string filename, int bufferSize, Guid profileId)
        {
            Profile profile = null;

            var client = new HttpClient
            {
                BaseAddress = new Uri(SurveyOnlineConstants.SurveyOnlineWepApi)
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(file, bufferSize);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            content.Add(streamContent, "image", filename);

            var result = await client.PostAsync(string.Format($"{PROFILE_CONTROLLER}/{profileId.ToString()}/pictures"), content);

            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();

                profile = JsonConvert.DeserializeObject<Profile>(response);
            }

            return profile;
        }

        public async Task<Profile> EditProfileAsync(Profile profile)
        {
            if (profile == null) return profile;

            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            Profile profileEdited = null;

            var result = await client.PutAsync(string.Format($"api/{PROFILE_CONTROLLER}/{profile.UserId.ToString()}"),
                new StringContent(JsonConvert.SerializeObject(profile), Encoding.Unicode, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                profileEdited = JsonConvert.DeserializeObject<Profile>(content);
            }

            return profileEdited;
        }
    }
}