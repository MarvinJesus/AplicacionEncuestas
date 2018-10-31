using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(2));//Missing parameters
                        break;
                    case 547:
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(4));//User not found
                        break;
                    default:
                        throw;
                }
                return new ManagerActionResult<Topic>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ManagerActionResult<Topic> EditTopic(Topic topic)
        {
            try
            {
                Topic existingTopic = _crudFactory.Retrieve<Topic>(topic);

                if (existingTopic != null)
                {
                    var result = _crudFactory.Update(topic);

                    if (result != 0)
                    {
                        _crudFactory.DeleteTopicsCategory(existingTopic);

                        if (topic.Categories != null)
                        {
                            var resultCategories = RegisterCategories(existingTopic.Id, topic.Categories);

                            if (resultCategories.Status == ManagerActionStatus.Created)
                            {
                                topic.Categories = resultCategories.Entity;
                            }
                            else
                            {
                                if (resultCategories.Status == ManagerActionStatus.Error)
                                {
                                    return new ManagerActionResult<Topic>(topic, ManagerActionStatus.Error,
                                    resultCategories.Exception);
                                }
                            }
                        }

                        return new ManagerActionResult<Topic>(topic, ManagerActionStatus.Updated);
                    }
                    else
                    {
                        return new ManagerActionResult<Topic>(topic, ManagerActionStatus.NothingModified);
                    }
                }

                return new ManagerActionResult<Topic>(topic, ManagerActionStatus.NotFound);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ManagerActionResult<Topic> DeleteTopic(Guid id, Guid userId)
        {
            try
            {
                var topicToDelete = new Topic { Id = id };

                Topic existingTopic = _crudFactory.Retrieve<Topic>(topicToDelete);

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

        public Topic GetTopic(Guid id)
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
            catch (System.Exception)
            {
                throw;
            }
        }

        public ICollection<Category> RetrieveCategoryByTopic(Topic entity)
        {
            try
            {
                return _crudFactory.RetrieveCategoryByTopic(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ManagerActionResult<ICollection<Category>> RegisterCategories(Guid topicId, ICollection<Category> categories)
        {
            try
            {
                var topic = new Topic { Id = topicId };
                var existingTopic = _crudFactory.Retrieve<Topic>(topic);
                ICollection<Category> categoriesCreated = null;

                if (existingTopic == null)
                    return new ManagerActionResult<ICollection<Category>>(categories, ManagerActionStatus.NotFound);

                categoriesCreated = new List<Category>();
                var actionResult = new ManagerActionResult<ICollection<Category>>(categoriesCreated, ManagerActionStatus.Created);

                foreach (var category in categories)
                {
                    try
                    {
                        var result = _crudFactory.CreateCategoryByTopic(existingTopic, category);

                        if (result != null)
                            categoriesCreated.Add(result);
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Number != 547) throw;

                        if (actionResult.Exception == null)
                        {
                            actionResult.Exception = ExceptionManager.GetInstance().Process(new BussinessException(7));
                            actionResult.Status = ManagerActionStatus.Error;
                        }
                    }
                }
                actionResult.Entity = categoriesCreated;

                return actionResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<Topic> GetTopics(string search)
        {
            try
            {
                return _crudFactory.SearchTopic(search);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }



    public interface ITopicManager
    {
        ManagerActionResult<Topic> RegisterTopic(Topic Topic);
        ManagerActionResult<Topic> EditTopic(Topic Topic);
        Topic GetTopic(Guid id);
        ICollection<Topic> GetTopicsByUser(Guid userId);
        ICollection<Topic> GetTopics();
        ManagerActionResult<Topic> DeleteTopic(Guid id, Guid userId);
        ICollection<Category> RetrieveCategoryByTopic(Topic entity);
        ManagerActionResult<ICollection<Category>> RegisterCategories(Guid topicId, ICollection<Category> categories);
        ICollection<Topic> GetTopics(string search);
    }
}
