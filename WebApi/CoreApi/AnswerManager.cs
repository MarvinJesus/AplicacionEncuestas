using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System.Collections.Generic;

namespace CoreApi
{
    public class AnswerManager : IAnswerManager
    {
        private QuestionCrudFactory _questionCrudFactory { get; set; }
        private AnswerCrudFactory _crudFactory { get; set; }

        public AnswerManager()
        {
            _crudFactory = new AnswerCrudFactory();
            _questionCrudFactory = new QuestionCrudFactory();
        }

        public ManagerActionResult<Answer> RegisterAnswer(Answer answer)
        {
            try
            {
                var question = _questionCrudFactory.Retrieve<Question>(new Question { Id = answer.QuestionId });
                if (question != null)
                {
                    var newAnswer = _crudFactory.Create<Answer>(answer);

                    if (newAnswer != null)
                    {
                        return new ManagerActionResult<Answer>(newAnswer, ManagerActionStatus.Created);
                    }
                    else
                    {
                        return new ManagerActionResult<Answer>(newAnswer, ManagerActionStatus.NothingModified);
                    }
                }
                else
                {
                    return new ManagerActionResult<Answer>(answer, ManagerActionStatus.Error, ExceptionManager.GetInstance().Process(new BussinessException(5)));
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
                return new ManagerActionResult<Answer>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);
                return new ManagerActionResult<Answer>(null, ManagerActionStatus.Error, exception);
            }
        }

        public Answer GetAnswer(int id)
        {
            try
            {
                return _crudFactory.Retrieve<Answer>(new Answer { Id = id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Answer> GetAnswersByQuestion(int answerId)
        {
            try
            {
                return _crudFactory.GetAllAnswersByQuestion<Answer>(new Answer { QuestionId = answerId });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ManagerActionResult<Answer> DeleteAnswer(int id, int questionId)
        {
            try
            {
                var answer = new Answer { Id = id };

                var existingAnswer = _crudFactory.Retrieve<Answer>(answer);

                if (existingAnswer != null)
                {
                    if (existingAnswer.QuestionId == questionId)
                    {
                        var result = _crudFactory.Delete(existingAnswer);

                        if (result != 0)
                        {
                            return new ManagerActionResult<Answer>(null, ManagerActionStatus.Deleted);
                        }
                        else
                        {
                            return new ManagerActionResult<Answer>(null, ManagerActionStatus.NothingModified);
                        }
                    }
                    else
                    {
                        return new ManagerActionResult<Answer>(null, ManagerActionStatus.Error, null);
                    }
                }
                else
                {
                    return new ManagerActionResult<Answer>(null, ManagerActionStatus.NotFound, null);
                }
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);
                return new ManagerActionResult<Answer>(null, ManagerActionStatus.Error, exception);
            }
        }

        public ManagerActionResult<Answer> UpdateAnswer(Answer answer)
        {
            try
            {
                Answer existingAnswer = _crudFactory.Retrieve<Answer>(answer);

                if (existingAnswer != null)
                {
                    var result = _crudFactory.Update(answer);

                    if (result != 0)
                    {
                        return new ManagerActionResult<Answer>(answer, ManagerActionStatus.Updated);
                    }
                    else
                    {
                        return new ManagerActionResult<Answer>(answer, ManagerActionStatus.NothingModified);
                    }
                }
                return new ManagerActionResult<Answer>(answer, ManagerActionStatus.NotFound);
            }
            catch (System.Exception ex)
            {
                return new ManagerActionResult<Answer>(null, ManagerActionStatus.Error,
                    ExceptionManager.GetInstance().Process(ex));
            }
        }
    }

    public interface IAnswerManager
    {
        Answer GetAnswer(int id);
        ICollection<Answer> GetAnswersByQuestion(int answerId);
        ManagerActionResult<Answer> RegisterAnswer(Answer answer);
        ManagerActionResult<Answer> DeleteAnswer(int id, int questionId);
        ManagerActionResult<Answer> UpdateAnswer(Answer answer);
    }
}
