using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;

namespace CoreApi
{
    public class SurveyManager : ISurveyManager
    {
        private SurveyCrudFactory _crudFactory { get; set; }
        private TopicCrudFactory _topicCrudFactory { get; set; }

        public SurveyManager()
        {
            _crudFactory = new SurveyCrudFactory();
            _topicCrudFactory = new TopicCrudFactory();
        }

        public ManagerActionResult<Survey> RegisterSurvey(Guid topicId, Survey survey)
        {
            try
            {
                var topic = _topicCrudFactory.Retrieve<Topic>(new Topic { Id = topicId });

                if (topic == null)
                    return new ManagerActionResult<Survey>(survey, ManagerActionStatus.NotFound);

                var newSurvey = _crudFactory.Create<Survey>(survey);

                if (newSurvey != null)
                {
                    return new ManagerActionResult<Survey>(newSurvey, ManagerActionStatus.Created);
                }
                else
                {
                    return new ManagerActionResult<Survey>(survey, ManagerActionStatus.NothingModified, null);
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
                        throw;//Uncontrolled exception
                }

                return new ManagerActionResult<Survey>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ManagerActionResult<Survey> EditSurvey(Survey survey)
        {
            try
            {
                Survey existingSurvey = _crudFactory.Retrieve<Survey>(survey);

                if (existingSurvey != null)
                {
                    var result = _crudFactory.Update(survey);

                    if (result != 0)
                    {
                        return new ManagerActionResult<Survey>(survey, ManagerActionStatus.Updated);
                    }
                    else
                    {
                        return new ManagerActionResult<Survey>(survey, ManagerActionStatus.NothingModified);
                    }
                }

                return new ManagerActionResult<Survey>(survey, ManagerActionStatus.NotFound);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Survey>(null, ManagerActionStatus.Error, exception);
            }
        }

        public ManagerActionResult<Survey> DeleteSurvey(Guid id, Guid topicId)
        {
            try
            {
                var SurveyToDelete = new Survey { Id = id };

                Survey existingSurvey = _crudFactory.Retrieve<Survey>(new Survey { Id = id });

                if (existingSurvey != null)
                {
                    if (existingSurvey.TopicId == topicId)
                    {
                        var result = _crudFactory.Delete(SurveyToDelete);

                        if (result != 0)
                        {
                            return new ManagerActionResult<Survey>(null, ManagerActionStatus.Deleted);
                        }
                        else
                        {
                            return new ManagerActionResult<Survey>(null, ManagerActionStatus.NothingModified);
                        }
                    }

                    return new ManagerActionResult<Survey>(null, ManagerActionStatus.Error, null);
                }

                return new ManagerActionResult<Survey>(null, ManagerActionStatus.NotFound);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Survey>(null, ManagerActionStatus.Error, exception);
            }
        }

        public Survey GetSurvey(Guid id)
        {
            try
            {
                return _crudFactory.Retrieve<Survey>(new Survey { Id = id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Survey> GetSurveysByTopic(Guid topicId)
        {
            try
            {
                return _crudFactory.GetAllSurveysByTopic<Survey>(new Survey { TopicId = topicId });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Survey> GetSurveys()
        {
            try
            {
                return _crudFactory.RetrieveAll<Survey>();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface ISurveyManager
    {
        ManagerActionResult<Survey> RegisterSurvey(Guid topicId, Survey survey);
        ManagerActionResult<Survey> EditSurvey(Survey survey);
        Survey GetSurvey(Guid id);
        ICollection<Survey> GetSurveysByTopic(Guid surveyId);
        ICollection<Survey> GetSurveys();
        ManagerActionResult<Survey> DeleteSurvey(Guid id, Guid topicId);
    }
}
