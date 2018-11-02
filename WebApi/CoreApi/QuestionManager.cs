using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CoreApi
{
    public class QuestionManager : IQuestionManager
    {
        private QuestionCrudFactory _questionCrudFactory { get; set; }
        private SurveyCrudFactory _SurveyCrudFactory { get; set; }
        private IAnswerManager _AnswerManager { get; set; }

        public QuestionManager()
        {
            _questionCrudFactory = new QuestionCrudFactory();
            _SurveyCrudFactory = new SurveyCrudFactory();
            _AnswerManager = new AnswerManager();
        }

        public ManagerActionResult<Question> RegisterQuestion(Question question)
        {
            try
            {
                var Survey = _SurveyCrudFactory.Retrieve<Survey>(new Survey { Id = question.SurveyId });

                if (Survey != null)
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
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(2)); //Missing parameters
                        break;
                    default:
                        exception = ExceptionManager.GetInstance().Process(sqlEx); //Uncontrolled exception
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

        public ManagerActionResult<ICollection<Question>> RegisterQuestions(Guid surveyId, ICollection<Question> questions)
        {
            try
            {
                var survey = _SurveyCrudFactory.Retrieve<Survey>(new Survey { Id = surveyId });

                if (survey == null)
                {
                    return new ManagerActionResult<ICollection<Question>>(questions, ManagerActionStatus.NotFound);
                }

                ICollection<Question> questionsRegistered = new List<Question>();
                var actionResult = new ManagerActionResult<ICollection<Question>>(questionsRegistered, ManagerActionStatus.Created);

                foreach (var question in questions)
                {
                    try
                    {
                        question.SurveyId = survey.Id;

                        var result = RegisterQuestion(question);

                        if (result.Status == ManagerActionStatus.Created)
                        {
                            if (question.Answers != null)
                            {
                                var answersResult = _AnswerManager.RegisterAnwers(result.Entity.Id, question.Answers);

                                if (answersResult.Status == ManagerActionStatus.Created)
                                {
                                    result.Entity.Answers = answersResult.Entity;
                                }
                                else
                                {
                                    if (answersResult.Status == ManagerActionStatus.Error)
                                    {
                                        actionResult.Status = ManagerActionStatus.Error;

                                        if (actionResult.Exception != null && !actionResult.Exception.AppMessage.Message.Contains("Respuestas"))
                                        {
                                            actionResult.Exception.AppMessage.Message += " Respuestas ";
                                        }
                                        else
                                        {
                                            if (actionResult.Exception == null)
                                                actionResult.Exception = answersResult.Exception;
                                        }
                                    }
                                }
                            }
                            questionsRegistered.Add(result.Entity);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Number == 201)
                        {
                            actionResult.Status = ManagerActionStatus.Error;

                            if (actionResult.Exception != null && !actionResult.Exception.AppMessage.Message.Contains("Preguntas"))
                            {
                                actionResult.Exception.AppMessage.Message += "Preguntas";
                            }
                            else
                            {
                                if (actionResult == null)
                                {
                                    actionResult.Exception = ExceptionManager.GetInstance().Process(new BussinessException(8));
                                    actionResult.Exception.AppMessage.Message += " Preguntas ";
                                }
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return actionResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<Question> GetQuestionsBySurvey(Guid surveyId)
        {
            try
            {
                return _questionCrudFactory.GetAllQuestionsBySurvey<Question>(new Question { SurveyId = surveyId });
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

        public ManagerActionResult<Question> DeleteQuestion(int id, Guid surveyId)
        {
            try
            {
                var existingQuestion = _questionCrudFactory.Retrieve<Question>(
                    new Question { Id = id });

                if (existingQuestion != null)
                {
                    if (existingQuestion.SurveyId == surveyId)
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
        ICollection<Question> GetQuestionsBySurvey(Guid surveyId);
        Question GetQuestion(int id);
        ManagerActionResult<Question> UpdateQuestion(Question question);
        ManagerActionResult<Question> DeleteQuestion(int id, Guid surveyId);
        ManagerActionResult<ICollection<Question>> RegisterQuestions(Guid surveyId, ICollection<Question> questions);
    }
}
