using CoreApi;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace WebApi.Controllers
{
    public class SurveysController : SurveyOnlineController
    {
        private ISurveyManager _manager { get; set; }

        public SurveysController(ISurveyManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("topics/{topicId}/surveys/{surveyId}")]
        [Route("surveys/{surveyId}")]
        public IHttpActionResult GetSurvey(Guid surveyId, Guid? topicId = null)
        {
            try
            {
                Survey Survey = new Survey();

                if (topicId == null)
                {
                    Survey = _manager.GetSurvey(surveyId);
                }
                else
                {
                    ICollection<Survey> surveys = _manager.GetSurveysByTopic((Guid)topicId);

                    if (surveys != null)
                        Survey = surveys.FirstOrDefault(t => t.Id == surveyId);
                }

                if (Survey != null)
                    return Ok(Survey);

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
        [Route("topics/{topicId}/surveys")]
        public IHttpActionResult GetSurveysByTopic(Guid topicId)
        {
            try
            {
                var surveys = _manager.GetSurveysByTopic(topicId);

                if (surveys.Count > 0)
                    return Ok(surveys);

                return NotFound();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        /*--------------------------------------------------
         *      WILL SEE ABOUT THIS METHOD
         -------------------------------------------------*/
        //[HttpGet]
        //[ScopeAuthorize("read")]
        //[Route("surveys")]
        //public IHttpActionResult GetSurveys()
        //{
        //    try
        //    {
        //        return base.Ok(_manager.GetSurveys());
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.GetInstance().Process(ex);
        //        return InternalServerError();
        //    }
        //}

        [HttpPost]
        [ScopeAuthorize("write")]
        [Route("topics/{topicId}/surveys")]
        public IHttpActionResult PostSurvey([FromBody] Survey Survey, Guid topicId)
        {
            try
            {
                if (Survey == null) return BadRequest();

                var result = _manager.RegisterSurvey(Survey);

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
        [ScopeAuthorize("write")]
        [Route("topics/{topicId}/surveys/{id}")]
        public IHttpActionResult PutSurvey(Guid topicId, Guid id, [FromBody]Survey survey)
        {
            try
            {
                if (survey == null)
                    return BadRequest();

                var result = _manager.EditSurvey(survey);

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
        [ScopeAuthorize("write")]
        [Route("topics/{topicId}/surveys/{id}")]
        public IHttpActionResult DeleteSurvey(Guid id, Guid topicId)
        {
            try
            {
                var result = _manager.DeleteSurvey(id, topicId);

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
