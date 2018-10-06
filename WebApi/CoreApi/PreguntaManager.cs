using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;

namespace CoreApi
{
    public class PreguntaManager : IPreguntaManager
    {
        private PreguntaCrudFactory _questionCrudFactory { get; set; }
        public TemaCrudFactory _topicCrudFactory { get; set; }

        public PreguntaManager()
        {
            _questionCrudFactory = new PreguntaCrudFactory();
            _topicCrudFactory = new TemaCrudFactory();
        }

        public ManagerActionResult<Pregunta> RegisterQuestion(Pregunta question)
        {
            try
            {
                var topic = _topicCrudFactory.Retrieve<Tema>(new Tema { Id = question.IdTema });

                if (topic != null)
                {
                    var newQuestion = _questionCrudFactory.Create<Pregunta>(question);

                    if (newQuestion != null)
                    {
                        return new ManagerActionResult<Pregunta>(newQuestion, ManagerActionStatus.Created);
                    }
                    else
                    {
                        return new ManagerActionResult<Pregunta>(newQuestion, ManagerActionStatus.NothingModified);
                    }
                }
                else
                {
                    return new ManagerActionResult<Pregunta>(question, ManagerActionStatus.Error, ExceptionManager.GetInstance().Process(new BussinessException(6)));
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                BussinessException exception;

                switch (sqlEx.Number)
                {
                    case 201:
                        //Missing parameters
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(2));
                        break;
                    default:
                        //Uncontrolled exception
                        exception = ExceptionManager.GetInstance().Process(sqlEx);
                        break;
                }
                return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);
                return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.Error, exception);
            }
        }

        public ICollection<Pregunta> GetQuestionsByTopic(int topicId)
        {
            try
            {
                return _questionCrudFactory.GetAllQuestionsByTopic<Pregunta>(new Pregunta { IdTema = topicId });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Pregunta> GetAllQuestions()
        {
            try
            {
                return _questionCrudFactory.RetrieveAll<Pregunta>();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }

    public interface IPreguntaManager
    {
        ICollection<Pregunta> GetAllQuestions();
        ManagerActionResult<Pregunta> RegisterQuestion(Pregunta question);
        ICollection<Pregunta> GetQuestionsByTopic(int topicId);
    }
}
