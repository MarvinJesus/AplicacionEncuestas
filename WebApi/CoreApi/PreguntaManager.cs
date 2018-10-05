using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Crud;
using Entities_POJO;

namespace CoreApi
{
   public class PreguntaManager
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
    
}
