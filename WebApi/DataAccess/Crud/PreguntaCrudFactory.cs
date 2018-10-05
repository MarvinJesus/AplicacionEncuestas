using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;

namespace DataAccess.Crud
{
  public  class PreguntaCrudFactory :CrudFactory
    {
        private PreguntaMapper mapper { get; set; }

        public PreguntaCrudFactory()
        {
            mapper = new PreguntaMapper();
            dao = SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override ICollection<T> RetrieveAll<T>()
        {
            var questionList = new List<T>();
            var questionsListResult = dao.ExecuteQueryProcedure(mapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (questionsListResult.Count > 0)
            {
                var objects = mapper.BuildObjects(questionsListResult);
                foreach (var c in objects)
                {
                    questionList.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return questionList;
        }

        public override int Update(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
