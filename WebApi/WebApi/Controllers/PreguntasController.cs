using CoreApi;
using Exceptions;
using System;
using System.Web.Http;

namespace WebApi.Controllers
{
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
    }
}
