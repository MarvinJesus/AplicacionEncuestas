using Entities_POJO;
using SurveyOnline.Web.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Web.Services
{
    public class CategoryService : Service
    {
        private readonly string CONTROLLER_NAME = "categories";

        public CategoryService(string accessToken) : base(accessToken)
        {
        }

        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            var client = SurveyOnlineHttpClient.GetHttpClient(_accessToken);
            ICollection<Category> categories = null;

            var result = await client.GetAsync(string.Concat("api/", CONTROLLER_NAME));

            if (result.IsSuccessStatusCode)
            {
                var categoryString = await result.Content.ReadAsStringAsync();

                categories = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Category>>(categoryString);
            }

            return categories;
        }
    }
}
