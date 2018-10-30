using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class CategoryCrudFacotory : CrudFactory
    {
        private CategoryMapper _mapper { get; set; }

        public CategoryCrudFacotory()
        {
            dao = SqlDao.GetInstance();
            _mapper = new CategoryMapper();
        }
        public override T Create<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override ICollection<T> RetrieveAll<T>()
        {
            var result = dao.ExecuteQueryProcedure(_mapper.GetRetriveAllStatement());
            var categoryList = new List<T>();

            if (result.Count > 0)
            {
                var objects = _mapper.BuildObjects(result);
                foreach (var c in objects)
                {
                    categoryList.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return categoryList;
        }

        public override int Update(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
