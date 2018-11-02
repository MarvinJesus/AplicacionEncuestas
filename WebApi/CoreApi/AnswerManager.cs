using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System.Collections.Generic;
using System.Linq;

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
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(2)); //Missing parameters
                        break;
                    default:
                        throw; //Uncontrolled exception
                }
                return new ManagerActionResult<Answer>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ManagerActionResult<ICollection<Answer>> RegisterAnwers(int questionId, ICollection<Answer> answers)
        {
            try
            {
                var existingQuestion = _questionCrudFactory.Retrieve<Question>(new Question { Id = questionId });

                if (existingQuestion == null)
                    return new ManagerActionResult<ICollection<Answer>>(answers, ManagerActionStatus.NotFound);

                ICollection<Answer> answersRegistered = new List<Answer>();
                var actionResult = new ManagerActionResult<ICollection<Answer>>(answersRegistered, ManagerActionStatus.Created);

                foreach (var answer in answers)
                {
                    try
                    {
                        answer.QuestionId = existingQuestion.Id;
                        var questionRegistered = _crudFactory.Create<Answer>(answer);

                        if (questionRegistered != null)
                        {
                            answersRegistered.Add(questionRegistered);
                        }
                        else
                        {
                            actionResult.Status = ManagerActionStatus.Error;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException sqlEx)
                    {
                        if (sqlEx.Number == 201)
                        {
                            actionResult.Status = ManagerActionStatus.Error;
                            actionResult.Exception = ExceptionManager.GetInstance().Process(new BussinessException(8));
                            actionResult.Exception.AppMessage.Message += " Respuestas ";
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return actionResult;
            }
            catch (System.Exception)
            {
                throw;
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
            catch (System.Exception)
            {
                throw;
            }
        }

        private void DeleteAnswers(ICollection<Answer> newAnswers, ICollection<Answer> answersToCompare)
        {
            foreach (var answer in answersToCompare)
            {
                var existingQuestion = newAnswers.FirstOrDefault(q => q.Id == answer.Id);

                if (existingQuestion == null)
                {
                    _crudFactory.Delete(answer);
                }
            }
        }

        public ManagerActionResult<Answer> DeleteAnswersByQuestion(int questionId)
        {
            try
            {
                var result = _crudFactory.DeleteAnswersByQuestion(new Answer { QuestionId = questionId });

                if (result != 0)
                {
                    return new ManagerActionResult<Answer>(null, ManagerActionStatus.Deleted);
                }
                else
                {
                    return new ManagerActionResult<Answer>(null, ManagerActionStatus.NothingModified);
                }
            }
            catch (System.Exception)
            {
                throw;
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
            catch (System.Exception)
            {
                throw;
            }
        }

        public ManagerActionResult<ICollection<Answer>> UpdateAnswers(int questionId, ICollection<Answer> answers)
        {
            try
            {
                if (answers == null)
                    return new ManagerActionResult<ICollection<Answer>>(answers, ManagerActionStatus.NothingModified);

                var answersList = _crudFactory.GetAllAnswersByQuestion<Answer>(new Answer { QuestionId = questionId });
                ICollection<Answer> newAnswersList = new List<Answer>();

                if (answersList != null)
                {
                    foreach (var answer in answers)
                    {
                        if (answer.Id == -1)
                        {
                            answer.QuestionId = questionId;
                            var result = RegisterAnswer(answer);

                            if (result.Status == ManagerActionStatus.Created)
                            {
                                newAnswersList.Add(result.Entity);
                            }
                        }
                        else
                        {
                            var existingAnswer = answersList.FirstOrDefault(a => a.Id == answer.Id);

                            if (existingAnswer != null)
                            {
                                var result = UpdateAnswer(answer);

                                if (result.Status == ManagerActionStatus.Updated)
                                {
                                    newAnswersList.Add(result.Entity);
                                }
                            }
                        }
                    }

                    DeleteAnswers(newAnswersList, answersList);

                    return new ManagerActionResult<ICollection<Answer>>(newAnswersList, ManagerActionStatus.Updated);
                }

                return new ManagerActionResult<ICollection<Answer>>(answers, ManagerActionStatus.NothingModified);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    public interface IAnswerManager
    {
        Answer GetAnswer(int id);
        ICollection<Answer> GetAnswersByQuestion(int answerId);
        ManagerActionResult<Answer> RegisterAnswer(Answer answer);
        ManagerActionResult<Answer> DeleteAnswer(int id, int questionId);
        ManagerActionResult<Answer> DeleteAnswersByQuestion(int questionId);
        ManagerActionResult<Answer> UpdateAnswer(Answer answer);
        ManagerActionResult<ICollection<Answer>> UpdateAnswers(int questionId, ICollection<Answer> answers);
        ManagerActionResult<ICollection<Answer>> RegisterAnwers(int questionId, ICollection<Answer> answers);
    }
}
