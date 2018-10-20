using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;

namespace CoreApi
{
    public class TopicManager : ITopicManager
    {
        private TopicCrudFactory _crudFactory { get; set; }
        private ProfileCrudFactory _ProfileCrudFactory { get; set; }

        public TopicManager()
        {
            _crudFactory = new TopicCrudFactory();
            _ProfileCrudFactory = new ProfileCrudFactory();
        }

        public ManagerActionResult<Topic> RegisterTopic(Topic Topic)
        {
            try
            {
                var newTopic = _crudFactory.Create<Topic>(Topic);

                if (newTopic != null)
                {
                    return new ManagerActionResult<Topic>(newTopic, ManagerActionStatus.Created);
                }
                else
                {
                    return new ManagerActionResult<Topic>(Topic, ManagerActionStatus.NothingModified, null);
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
                    case 547:
                        //User not found
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(4));
                        break;
                    default:
                        //Uncontrolled exception
                        exception = ExceptionManager.GetInstance().Process(sqlEx);
                        break;
                }
                return new ManagerActionResult<Topic>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Topic>(null, ManagerActionStatus.Error, exception);
            }
        }

        public ManagerActionResult<Topic> EditTopic(Topic Topic)
        {
            try
            {
                Topic existingTopic = _crudFactory.Retrieve<Topic>(Topic);

                if (existingTopic != null)
                {
                    var result = _crudFactory.Update(Topic);

                    if (result != 0)
                    {
                        return new ManagerActionResult<Topic>(Topic, ManagerActionStatus.Updated);
                    }
                    else
                    {
                        return new ManagerActionResult<Topic>(Topic, ManagerActionStatus.NothingModified);
                    }
                }

                return new ManagerActionResult<Topic>(Topic, ManagerActionStatus.NotFound);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Topic>(null, ManagerActionStatus.Error, exception);
            }
        }

        public ManagerActionResult<Topic> DeleteTopic(int id, Guid userId)
        {
            try
            {
                var topicToDelete = new Topic { Id = id };

                Topic existingTopic = _crudFactory.Retrieve<Topic>(new Topic { Id = id });

                if (existingTopic != null)
                {
                    if (existingTopic.UserId == userId)
                    {
                        var result = _crudFactory.Delete(topicToDelete);

                        if (result != 0)
                        {
                            return new ManagerActionResult<Topic>(null, ManagerActionStatus.Deleted);
                        }
                        else
                        {
                            return new ManagerActionResult<Topic>(null, ManagerActionStatus.NothingModified);
                        }
                    }

                    return new ManagerActionResult<Topic>(null, ManagerActionStatus.Error, null);
                }

                return new ManagerActionResult<Topic>(null, ManagerActionStatus.NotFound);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Topic>(null, ManagerActionStatus.Error, exception);
            }
        }

        public Topic GetTopic(int id)
        {
            try
            {
                return _crudFactory.Retrieve<Topic>(new Topic { Id = id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Topic> GetTopicsByUser(Guid userId)
        {
            try
            {
                return _crudFactory.GetAllTopicsByUser<Topic>(new Topic { UserId = userId });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Topic> GetTopics()
        {
            try
            {
                return _crudFactory.RetrieveAll<Topic>();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }



    public interface ITopicManager
    {
        ManagerActionResult<Topic> RegisterTopic(Topic Topic);
        ManagerActionResult<Topic> EditTopic(Topic Topic);
        Topic GetTopic(int id);
        ICollection<Topic> GetTopicsByUser(Guid userId);
        ICollection<Topic> GetTopics();
        ManagerActionResult<Topic> DeleteTopic(int id, Guid userId);
    }
}
