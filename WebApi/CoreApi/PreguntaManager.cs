using DataAccess.Crud;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace CoreApi
{
    public class PreguntaManager : IPreguntaManager
    {
        private PreguntaCrudFactory Preguntacrud { get; set; }

        public PreguntaManager()
        {
            Preguntacrud = new PreguntaCrudFactory();
        }

        public ICollection<Pregunta> GetAllQuestions()
        {
            try
            {
                return Preguntacrud.RetrieveAll<Pregunta>();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }

    public interface IPreguntaManager
    {
        ICollection<Pregunta> GetAllQuestions();
    }
}
