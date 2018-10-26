using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;

namespace CoreApi
{
    public class QuestionManager : IQuestionManager
    {
        private QuestionCrudFactory _questionCrudFactory { get; set; }
        public TopicCrudFactory _topicCrudFactory { get; set; }

        public QuestionManager()
        {
            _questionCrudFactory = new QuestionCrudFactory();
            _topicCrudFactory = new TopicCrudFactory();
        }

        public ManagerActionResult<Question> RegisterQuestion(Question question)
        {
            try
            {
                var topic = _topicCrudFactory.Retrieve<Topic>(new Topic { Id = question.TopicId });

                if (topic != null)
                {
                    var newQuestion = _questionCrudFactory.Create<Question>(question);

                    if (newQuestion != null)
                    {
                        return new ManagerActionResult<Question>(newQuestion, ManagerActionStatus.Created);
                    }
                    else
                    {
                        return new ManagerActionResult<Question>(newQuestion, ManagerActionStatus.NothingModified);
                    }
                }
                else
                {
                    return new ManagerActionResult<Question>(question, ManagerActionStatus.Error, ExceptionManager.GetInstance().Process(new BussinessException(6)));
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
                return new ManagerActionResult<Question>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                return new ManagerActionResult<Question>(null, ManagerActionStatus.Error,
                    ExceptionManager.GetInstance().Process(ex));
            }
        }

        public ICollection<Question> GetQuestionsByTopic(Guid topicId)
        {
            try
            {
                return _questionCrudFactory.GetAllQuestionsByTopic<Question>(new Question { TopicId = topicId });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Question> GetAllQuestions()
        {
            try
            {
                return _questionCrudFactory.RetrieveAll<Question>();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Question GetQuestion(int id)
        {
            try
            {
                return _questionCrudFactory.Retrieve<Question>(new Question { Id = id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ManagerActionResult<Question> UpdateQuestion(Question question)
        {
            try
            {
                Question existingQuestion = _questionCrudFactory.Retrieve<Question>(question);

                if (existingQuestion != null)
                {
                    var result = _questionCrudFactory.Update(question);

                    if (result != 0)
                    {
                        return new ManagerActionResult<Question>(question, ManagerActionStatus.Updated);
                    }
                    else
                    {
                        return new ManagerActionResult<Question>(question, ManagerActionStatus.NothingModified);
                    }
                }

                return new ManagerActionResult<Question>(question, ManagerActionStatus.NotFound);
            }
            catch (System.Exception ex)
            {
                return new ManagerActionResult<Question>(null, ManagerActionStatus.Error,
                    ExceptionManager.GetInstance().Process(ex));
            }
        }

        public ManagerActionResult<Question> DeleteQuestion(int id, Guid topicId)
        {
            try
            {
                var existingQuestion = _questionCrudFactory.Retrieve<Question>(
                    new Question { Id = id });

                if (existingQuestion != null)
                {
                    if (existingQuestion.TopicId == topicId)
                    {
                        var result = _questionCrudFactory.Delete(existingQuestion);

                        if (result != 0)
                        {
                            return new ManagerActionResult<Question>(null, ManagerActionStatus.Deleted);
                        }
                        else
                        {
                            return new ManagerActionResult<Question>(null, ManagerActionStatus.NothingModified);
                        }
                    }
                    else
                    {
                        return new ManagerActionResult<Question>(null, ManagerActionStatus.Error, null);
                    }
                }
                else
                {
                    return new ManagerActionResult<Question>(null, ManagerActionStatus.NotFound, null);
                }
            }
            catch (System.Exception ex)
            {
                return new ManagerActionResult<Question>(null, ManagerActionStatus.Error,
                    ExceptionManager.GetInstance().Process(ex));
            }
        }
    }

    public interface IQuestionManager
    {
        ICollection<Question> GetAllQuestions();
        ManagerActionResult<Question> RegisterQuestion(Question question);
        ICollection<Question> GetQuestionsByTopic(Guid topicId);
        Question GetQuestion(int id);
        ManagerActionResult<Question> UpdateQuestion(Question question);
        ManagerActionResult<Question> DeleteQuestion(int id, Guid topicId);
    }
}
