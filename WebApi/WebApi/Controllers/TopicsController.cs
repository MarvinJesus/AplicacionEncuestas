using CoreApi;
using DataAccess.Factory;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Thinktecture.IdentityModel.WebApi;
using WebApi.Helper;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class TopicsController : SurveyOnlineController
    {
        private ITopicManager _manager { get; set; }
        private const int MAX_PAGE_SIZE = 10;
        private TopicFactory TopicFactory { get; set; }
        private const string CATEGORIES_PROPERTY = "categories";
        private const string TOTAL_SURVEY_PROPERTY = "totalsurvey";

        public TopicsController(ITopicManager manager)
        {
            TopicFactory = new TopicFactory();
            _manager = manager;
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("profiles/{userId}/topics/{topicId}")]
        [Route("topics/{topicId}")]
        public IHttpActionResult GetTopic(Guid topicId, Guid? userId = null, string fields = null)
        {
            try
            {
                Topic topic = null;

                if (userId == null)
                {
                    topic = _manager.GetTopic(topicId);
                }
                else
                {
                    ICollection<Topic> topics = _manager.GetTopicsByUser((Guid)userId);

                    if (topics != null)
                        topic = topics.FirstOrDefault(t => t.Id == topicId);
                }

                if (topic != null)
                {
                    if (fields != null)
                    {
                        var listOfFields = fields.ToLower().Split(',').ToList();

                        if (listOfFields.Contains(CATEGORIES_PROPERTY))
                        {
                            topic.Categories = _manager.RetrieveCategoryByTopic(topic);
                        }

                        if (listOfFields.Contains(TOTAL_SURVEY_PROPERTY))
                        {
                            topic.TotalSurvey = _manager.GetTotalSurveyByTopic(topic.Id);
                        }

                        return Ok(TopicFactory.CreateDataShapeObject(topic, listOfFields));
                    }
                    else
                    {
                        return Ok(TopicFactory.CreateDataShapeObject(topic));
                    }
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("profiles/{userId}/topics")]
        public IHttpActionResult GetTopicsByUser(Guid userId, string filters = null, string fields = null)
        {
            try
            {
                var topics = _manager.GetTopicsByUser(userId);
                IQueryable<Topic> topicsList = null;

                if (topics.Count < 1) return NotFound();

                if (filters != null || fields != null && fields.Contains(CATEGORIES_PROPERTY))
                {
                    foreach (var topic in topics)
                    {
                        topic.Categories = _manager.RetrieveCategoryByTopic(topic);
                    }
                }

                if (filters != null)
                {
                    topicsList = topics.AsQueryable().ApplyFilter(filters);
                }
                else
                {
                    topicsList = topics.AsQueryable();
                }

                if (fields != null)
                {
                    var listOfFields = fields.ToLower().Split(',').ToList();
                    if (listOfFields.Contains(TOTAL_SURVEY_PROPERTY))
                    {
                        foreach (var topic in topicsList)
                        {
                            topic.TotalSurvey = _manager.GetTotalSurveyByTopic(topic.Id);
                        }
                    }

                    return Ok(topicsList.Select(t => TopicFactory.CreateDataShapeObject(t, listOfFields)));
                }
                else
                {
                    var list = topicsList.Select(t => TopicFactory.CreateDataShapeObject(t));
                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("topics", Name = "TopicsList")]
        public IHttpActionResult GetTopics(string sort = "Id", string filters = null,
            int page = 1, int pageSize = MAX_PAGE_SIZE, string fields = null, string search = null)
        {
            try
            {
                List<string> listOfFields = null;
                ICollection<Topic> topics = null;

                if (search != null)
                {
                    topics = _manager.GetTopics(search);
                }
                else
                {
                    topics = _manager.GetTopics();
                }

                if (topics == null) return Ok(topics);

                if (filters != null || fields != null)
                {
                    foreach (var topic in topics)
                    {
                        topic.Categories = _manager.RetrieveCategoryByTopic(topic);
                    }
                }

                if (fields != null)
                {
                    listOfFields = new List<string>();
                    listOfFields = fields.ToLower().Split(',').ToList();
                }

                var topicList = topics.AsQueryable()
                    .ApplyFilter(filters);

                HttpContext.Current.Response.Headers.Add("X-Pagination", Paging.GetInstance().Page(page, pageSize, MAX_PAGE_SIZE, topicList,
                    "TopicsList", new UrlHelper(Request), sort, filters, fields, search));

                var topicResult = topicList
                    .ApplySort(sort)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList();

                if (fields != null)
                {
                    if (listOfFields.Contains(TOTAL_SURVEY_PROPERTY))
                    {
                        foreach (var topic in topicResult)
                        {
                            topic.TotalSurvey = _manager.GetTotalSurveyByTopic(topic.Id);
                        }
                    }
                    var list = topicResult.Select(top => TopicFactory.CreateDataShapeObject(top, listOfFields));
                    return Ok(list);
                }
                else
                {
                    return Ok(topicResult.Select(top => TopicFactory.CreateDataShapeObject(top)));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [ScopeAuthorize("write")]
        [Route("profiles/{userId}/topics")]
        public IHttpActionResult PostTopic([FromBody] Topic topic, Guid userId)
        {
            try
            {
                if (!userId.Equals(GetProfileId())) return Unauthorized();

                if (topic == null) return BadRequest();

                var result = _manager.RegisterTopic(topic);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                {
                    if (topic.Categories != null)
                    {
                        var resultCategories = _manager.RegisterCategories(result.Entity.Id, topic.Categories);

                        if (resultCategories.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                        {
                            return BadRequest(string.Format("El usuario se creo exitosamente pero {0}",
                                resultCategories.Exception.AppMessage.Message));
                        }
                        result.Entity.Categories = resultCategories.Entity;
                    }

                    return Created(Request.RequestUri + "/" + result.Entity.Id.ToString(), result.Entity);
                }

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                {
                    return BadRequest(result.Exception.AppMessage.Message);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [ScopeAuthorize("write")]
        [Route("topics/{topicId}/categories")]
        public IHttpActionResult PostTopicCategories(Guid topicId, ICollection<Category> categories)
        {
            try
            {
                if (categories == null) return BadRequest();

                var result = _manager.RegisterCategories(topicId, categories);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                    return Created(Request.RequestUri, result.Entity);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                {
                    return BadRequest(result.Exception.AppMessage.Message);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPut]
        [ScopeAuthorize("write")]
        [Route("profiles/{profileId}/topics/{id}")]
        public IHttpActionResult PutTopic(Guid profileId, Guid id, [FromBody]Topic topic)
        {
            try
            {
                if (!profileId.Equals(GetProfileId())) return Unauthorized();

                if (topic == null)
                    return BadRequest();

                var result = _manager.EditTopic(topic);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Updated)
                    return Ok(result.Entity);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                    return BadRequest(string.Concat("El tema pudo ser actualizado pero {0}", result.Exception.AppMessage.Message));

                return BadRequest();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [ScopeAuthorize("write")]
        [Route("profiles/{userId}/topics/{id}")]
        public IHttpActionResult DeleteTopic(Guid id, Guid userId)
        {
            try
            {
                if (userId.Equals(GetProfileId())) return Unauthorized();

                var result = _manager.DeleteTopic(id, userId);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Deleted)
                    return StatusCode(System.Net.HttpStatusCode.NoContent);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                return BadRequest();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }
    }
}