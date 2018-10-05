using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;
using System.Collections.Generic;

namespace CoreApi
{
    public class TemaManager : ITemaManager
    {
        private TemaCrudFactory _crudFactory { get; set; }
        private UsuarioCrudFactory _usuarioCrudFactory { get; set; }

        public TemaManager()
        {
            _crudFactory = new TemaCrudFactory();
            _usuarioCrudFactory = new UsuarioCrudFactory();
        }

        public ManagerActionResult<Tema> RegistrarTema(Tema tema)
        {
            try
            {
                var newTema = _crudFactory.Create<Tema>(tema);

                if (newTema != null)
                {
                    return new ManagerActionResult<Tema>(newTema, ManagerActionStatus.Created);
                }
                else
                {
                    return new ManagerActionResult<Tema>(tema, ManagerActionStatus.NothingModified, null);
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
                    case 547:
                        //User not found
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(4));
                        break;
                    default:
                        //Uncontrolled exception
                        exception = ExceptionManager.GetInstance().Process(sqlEx);
                        break;
                }
                return new ManagerActionResult<Tema>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Tema>(null, ManagerActionStatus.Error, exception);
            }
        }

        public ManagerActionResult<Tema> ActualizarTema(Tema tema)
        {
            try
            {
                Tema existingTopic = _crudFactory.Retrieve<Tema>(tema);

                if (existingTopic == null)
                {
                    var result = _crudFactory.Update(tema);

                    if (result != 0)
                    {
                        return new ManagerActionResult<Tema>(tema, ManagerActionStatus.Updated);
                    }
                    else
                    {
                        return new ManagerActionResult<Tema>(tema, ManagerActionStatus.NothingModified);
                    }
                }

                return new ManagerActionResult<Tema>(tema, ManagerActionStatus.NotFound);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Tema>(null, ManagerActionStatus.Error, exception);
            }
        }

        public Tema GetTopic(int id)
        {
            try
            {
                return _crudFactory.Retrieve<Tema>(new Tema { Id = id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Tema> GetTopicsByUser(int userId)
        {
            try
            {
                return _crudFactory.GetAllTemasByUser<Tema>(new Tema { UsuarioId = userId });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }



    public interface ITemaManager
    {
        ManagerActionResult<Tema> RegistrarTema(Tema tema);
        ManagerActionResult<Tema> ActualizarTema(Tema tema);
        Tema GetTopic(int id);
        ICollection<Tema> GetTopicsByUser(int userId);
    }
}
