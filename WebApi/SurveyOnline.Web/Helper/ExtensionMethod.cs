using Entities_POJO;
using System.Collections.Generic;
using System.Linq;

namespace SurveyOnline.Web.Helper
{
    public static class ExtensionMethod
    {
        public static ICollection<Category> ReturnEquals(this ICollection<Category> categories,
            IEnumerable<string> categoriesName)
        {
            if (categoriesName == null) return null;

            var newCategoriesList = new List<Category>();

            foreach (var category in categories)
            {
                if (categoriesName.Contains(category.Name))
                {
                    newCategoriesList.Add(category);
                }
            }

            return newCategoriesList;
        }

        public static ICollection<Category> ReturnDiferents(this ICollection<Category> categories,
        IEnumerable<string> categoriesName)
        {
            if (categoriesName == null) return categories;

            var newCategoriesList = new List<Category>();

            foreach (var category in categories)
            {
                if (!categoriesName.Contains(category.Name))
                {
                    newCategoriesList.Add(category);
                }
            }

            return newCategoriesList;
        }
    }
}