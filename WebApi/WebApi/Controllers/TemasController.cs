using CoreApi;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class TemasController : ApiController
    {
        private ITemaManager _manager { get; set; }
        private IUsuarioManager _usuarioManager { get; set; }

        public TemasController(ITemaManager manager, IUsuarioManager usuarioManager)
        {
            _manager = manager;
            _usuarioManager = usuarioManager;
        }

        [HttpGet]
        [Route("usuarios/{userId}/temas/{topicId}")]
        [Route("temas/{topicId}")]
        public IHttpActionResult GetTopic(int topicId, int? userId = null)
        {
            try
            {
                Tema topic = null;

                if (userId == null)
                {
                    topic = _manager.GetTopic(topicId);
                }
                else
                {
                    ICollection<Tema> topics = _manager.GetTopicsByUser((int)userId);

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
        [Route("usuarios/{userId}/temas")]
        public IHttpActionResult GetTopicsByUser(int userId)
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
        [Route("temas")]
        public IHttpActionResult GetTopics()
        {
            try
            {
                var topics = _manager.GetTopics();

                return Ok(topics);
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("temas")]
        public IHttpActionResult PostTopic([FromBody] Tema tema)
        {
            try
            {
                if (tema == null)
                    return BadRequest();

                var result = _manager.RegistrarTema(tema);

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
        [Route("temas/{id}")]
        public IHttpActionResult PutTopic(int id, [FromBody]Tema topic)
        {
            try
            {
                if (topic == null)
                    return BadRequest();

                var result = _manager.ActualizarTema(topic);

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
        [Route("usuarios/{userId}/temas/{id}")]
        public IHttpActionResult DeleteTopic(int id, int userId)
        {
            try
            {
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