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
                return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.Error,
                    ExceptionManager.GetInstance().Process(ex));
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

        public Pregunta GetQuestion(int id)
        {
            try
            {
                return _questionCrudFactory.Retrieve<Pregunta>(new Pregunta { Id = id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ManagerActionResult<Pregunta> UpdateQuestion(Pregunta question)
        {
            try
            {
                Pregunta existingQuestion = _questionCrudFactory.Retrieve<Pregunta>(question);

                if (existingQuestion != null)
                {
                    var result = _questionCrudFactory.Update(question);

                    if (result != 0)
                    {
                        return new ManagerActionResult<Pregunta>(question, ManagerActionStatus.Updated);
                    }
                    else
                    {
                        return new ManagerActionResult<Pregunta>(question, ManagerActionStatus.NothingModified);
                    }
                }

                return new ManagerActionResult<Pregunta>(question, ManagerActionStatus.NotFound);
            }
            catch (System.Exception ex)
            {
                return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.Error,
                    ExceptionManager.GetInstance().Process(ex));
            }
        }

        public ManagerActionResult<Pregunta> DeleteQuestion(int id, int topicId)
        {
            try
            {
                var existingQuestion = _questionCrudFactory.Retrieve<Pregunta>(
                    new Pregunta { Id = id });

                if (existingQuestion != null)
                {
                    if (existingQuestion.IdTema == topicId)
                    {
                        var result = _questionCrudFactory.Delete(existingQuestion);

                        if (result != 0)
                        {
                            return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.Deleted);
                        }
                        else
                        {
                            return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.NothingModified);
                        }
                    }
                    else
                    {
                        return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.Error, null);
                    }
                }
                else
                {
                    return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.NotFound, null);
                }
            }
            catch (System.Exception ex)
            {
                return new ManagerActionResult<Pregunta>(null, ManagerActionStatus.Error,
                    ExceptionManager.GetInstance().Process(ex));
            }
        }
    }

    public interface IPreguntaManager
    {
        ICollection<Pregunta> GetAllQuestions();
        ManagerActionResult<Pregunta> RegisterQuestion(Pregunta question);
        ICollection<Pregunta> GetQuestionsByTopic(int topicId);
        Pregunta GetQuestion(int id);
        ManagerActionResult<Pregunta> UpdateQuestion(Pregunta question);
        ManagerActionResult<Pregunta> DeleteQuestion(int id, int topicId);
    }
}
