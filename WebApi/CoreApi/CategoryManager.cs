using DataAccess.Crud;
using Entities_POJO;
using System.Collections.Generic;

namespace CoreApi
{
    public class CategoryManager : ICategoryManager
    {
        private CategoryCrudFacotory _crudFactory { get; set; }

        public CategoryManager()
        {
            _crudFactory = new CategoryCrudFacotory();
        }

        public ICollection<Category> GetCategories()
        {
            try
            {
                return _crudFactory.RetrieveAll<Category>();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    public interface ICategoryManager
    {
        ICollection<Category> GetCategories();
    }
}
