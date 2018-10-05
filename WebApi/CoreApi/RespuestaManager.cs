using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System.Collections.Generic;

namespace CoreApi
{
    public class RespuestaManager : IRespuestaManager
    {
        private PreguntaCrudFactory _questionCrudFactory { get; set; }
        private RespuestaCrudFactory _crudFactory { get; set; }

        public RespuestaManager()
        {
            _crudFactory = new RespuestaCrudFactory();
            _questionCrudFactory = new PreguntaCrudFactory();
        }

        public ManagerActionResult<Respuesta> RegisterAnswer(Respuesta answer)
        {
            try
            {
                var question = _questionCrudFactory.Retrieve<Pregunta>(new Pregunta { Id = answer.IdPregunta });
                if (question != null)
                {
                    var newAnswer = _crudFactory.Create<Respuesta>(answer);

                    if (newAnswer != null)
                    {
                        return new ManagerActionResult<Respuesta>(newAnswer, ManagerActionStatus.Created);
                    }
                    else
                    {
                        return new ManagerActionResult<Respuesta>(newAnswer, ManagerActionStatus.NothingModified);
                    }
                }
                else
                {
                    return new ManagerActionResult<Respuesta>(answer, ManagerActionStatus.Error, ExceptionManager.GetInstance().Process(new BussinessException(5)));
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
                    default:
                        //Uncontrolled exception
                        exception = ExceptionManager.GetInstance().Process(sqlEx);
                        break;
                }
                return new ManagerActionResult<Respuesta>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);
                return new ManagerActionResult<Respuesta>(null, ManagerActionStatus.Error, exception);
            }
        }

        public Respuesta GetAnswer(int id)
        {
            try
            {
                return _crudFactory.Retrieve<Respuesta>(new Respuesta { Id = id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Respuesta> GetAnswersByQuestionId(int answerId)
        {
            try
            {
                return _crudFactory.GetAllAnswersByQuestion<Respuesta>(new Respuesta { IdPregunta = answerId });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface IRespuestaManager
    {
        Respuesta GetAnswer(int id);
        ICollection<Respuesta> GetAnswersByQuestionId(int answerId);
        ManagerActionResult<Respuesta> RegisterAnswer(Respuesta answer);
    }
}
