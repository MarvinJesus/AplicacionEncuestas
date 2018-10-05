using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoreApi;
using Exceptions;

namespace WebApi.Controllers
{
    public class PreguntasController : ApiController
    {
        public PreguntaManager preguntaManager { get; set; }

        public PreguntasController()
        {
            preguntaManager = new PreguntaManager();
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var questions = preguntaManager.GetAllQuestions();
                return Ok(questions);
            }
            catch (Exception e)
            {
                ExceptionManager.GetInstance().Process(e);
                return InternalServerError();
            }
        }
    }
}
