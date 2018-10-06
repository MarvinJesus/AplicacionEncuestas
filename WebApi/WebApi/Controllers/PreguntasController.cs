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
    public class PreguntasController : ApiController
    {
        public IPreguntaManager _manager { get; set; }

        public PreguntasController(IPreguntaManager preguntaManager)
        {
            _manager = preguntaManager;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var questions = _manager.GetAllQuestions();
                return Ok(questions);
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("preguntas")]
        public IHttpActionResult PostQuestion([FromBody]Pregunta question)
        {
            try
            {
                if (question == null)
                    return BadRequest();

                var result = _manager.RegisterQuestion(question);

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
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("temas/{id}/preguntas")]
        public IHttpActionResult GetQuestionsbyTopic(int id)
        {
            try
            {
                var questions = _manager.GetQuestionsByTopic(id);

                if (questions?.Count > 0)
                    return Ok(questions);

                return NotFound();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("temas/{topicId}/preguntas/{id}")]
        [Route("preguntas/{id}")]
        public IHttpActionResult GetQuestion(int id, int? topicId = null)
        {
            try
            {
                Pregunta question = null;

                if (topicId == null)
                {
                    question = _manager.GetQuestion(id);
                }
                else
                {
                    ICollection<Pregunta> questions = _manager.GetQuestionsByTopic((int)topicId);

                    if (questions?.Count > 0)
                    {
                        question = questions.FirstOrDefault(a => a.Id == id);
                    }
                }

                if (question != null)
                    return Ok(question);

                return NotFound();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("preguntas/{id}")]
        public IHttpActionResult PutQuestion(int id, [FromBody]Pregunta question)
        {
            try
            {
                if (question == null)
                    return BadRequest();

                var result = _manager.UpdateQuestion(question);

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
        [Route("temas/{topicId}/preguntas/{id}")]
        public IHttpActionResult DeleteAnswer(int id, int topicId)
        {
            try
            {
                var result = _manager.DeleteQuestion(id, topicId);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Deleted)
                    return StatusCode(System.Net.HttpStatusCode.NoContent);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error && result.Exception != null)
                    return InternalServerError();

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }
    }
}
