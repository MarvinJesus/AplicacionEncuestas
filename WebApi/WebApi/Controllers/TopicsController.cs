using CoreApi;
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
    //[Authorize]
    public class TopicsController : SurveyOnlineController
    {
        private ITopicManager _manager { get; set; }
        private const int MAX_PAGE_SIZE = 10;

        public TopicsController(ITopicManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("profiles/{userId}/topics/{topicId}")]
        [Route("topics/{topicId}")]
        public IHttpActionResult GetTopic(Guid topicId, Guid? userId = null)
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
                    return Ok(topic);

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
        public IHttpActionResult GetTopicsByUser(Guid userId)
        {
            try
            {
                var topics = _manager.GetTopicsByUser(userId);

                if (topics.Count > 0)
                    return Ok(topics);

                return NotFound();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        //[ScopeAuthorize("read")]
        [Route("topics", Name = "TopicsList")]
        public IHttpActionResult GetTopics(string search, string sort = "Id", string category = null,
            int page = 1, int pageSize = MAX_PAGE_SIZE)
        {
            try
            {
                ICollection<Topic> topics = null;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    topics = _manager.GetTopics(search);
                }
                else
                {
                    topics = _manager.GetTopics();
                }

                if (topics == null) return Ok(topics);

                var topicList = topics
                    .AsQueryable<Topic>()
                    .ApplyFilter(category);

                if (pageSize > MAX_PAGE_SIZE)
                {
                    pageSize = MAX_PAGE_SIZE;
                }

                var totalCount = topicList.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);

                var prevLink = page > 1 ? urlHelper.Link("Topics",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        sort = sort,
                        category = category
                    }) : "";
                var nextLink = page < totalPages ? urlHelper.Link("TopicsList",
                    new
                    {
                        page = page + 1,
                        pageSize = pageSize,
                        sort = sort,
                        category = category
                    }) : "";

                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    previousLink = prevLink,
                    nextLink = nextLink
                };

                HttpContext.Current.Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

                var topicResult = topicList
                    .ApplySort<Topic>(sort)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList();

                return Ok(topicResult);
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