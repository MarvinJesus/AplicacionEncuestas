﻿using CoreApi;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class TopicsController : ApiController
    {
        private ITopicManager _manager { get; set; }

        public TopicsController(ITopicManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Route("profiles/{userId}/topics/{topicId}")]
        [Route("topics/{topicId}")]
        public IHttpActionResult GetTopic(int topicId, Guid? userId = null)
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
        [Route("topics")]
        public IHttpActionResult GetTopics()
        {
            try
            {
                return base.Ok(_manager.GetTopics());
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("topics")]
        public IHttpActionResult PostTopic([FromBody] Topic Topic)
        {
            try
            {
                if (Topic == null)
                    return BadRequest();

                var result = _manager.RegisterTopic(Topic);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                    return Created(Request.RequestUri + "/" + result.Entity.Id.ToString(), result.Entity);

                if (result.Exception != null)
                {
                    if (result.Exception.Code == 1)
                    {
                        return InternalServerError();
                    }
                    else
                    {
                        return BadRequest(result.Exception.AppMessage.Message);
                    }
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
        [Route("topics/{id}")]
        public IHttpActionResult PutTopic(int id, [FromBody]Topic topic)
        {
            try
            {
                if (topic == null)
                    return BadRequest();

                var result = _manager.EditTopic(topic);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Updated)
                    return Ok(result.Entity);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                    return InternalServerError();

                return BadRequest();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("profiles/{userId}/topics/{id}")]
        public IHttpActionResult DeleteTopic(int id, Guid userId)
        {
            try
            {
                var result = _manager.DeleteTopic(id, userId);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Deleted)
                    return StatusCode(System.Net.HttpStatusCode.NoContent);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error && result.Exception != null)
                    return InternalServerError();

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