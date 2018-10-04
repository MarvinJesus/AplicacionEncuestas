using DataAccess.Crud;
using DataAccess.Dao;
using Entities_POJO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execptions
{
   public class ExceptionManager
    {
       

        private static ExceptionManager instance;

        private static Dictionary<int, ApplicationMessage> messages = new Dictionary<int, ApplicationMessage>();

        private ExceptionManager()
        {
            LoadMessages();
        }

        public static ExceptionManager GetInstance()
        {
            if (instance == null)
                instance = new ExceptionManager();

            return instance;
        }

        public void Process(Exception ex)

        {
            var bex = new BussinessException();

            if (ex.GetType() == typeof(BussinessException))
            {
                bex = (BussinessException)ex;
            }
            else
            {
                bex = new BussinessException(0, ex);
            }

            ProcessBussinesException(bex);
        }

        private void ProcessBussinesException(BussinessException bex)
        {
            var today = string.Format("{0:yyyy-MM-dd_HH_mm_ss}_log.txt", DateTime.Now);


            var message = bex.Message + "\n" + bex.StackTrace + "\n";

            if (bex.InnerException != null)
                message += bex.InnerException.Message + "\n" + bex.InnerException.StackTrace;

          
            bex.AppMessage = GetMessage(bex);

            RegisterException(bex);

            throw bex;
        }

        public void RegisterException(BussinessException ex)
        {
            var operation = new SqlOperation { ProcedureName = "INS_Exception" };

            operation.AddVarcharParam("Dia", DateTime.Now.ToString("yyyy-MM-dd"));
            operation.AddVarcharParam("Hora", DateTime.Now.ToString("HH:mm:ss"));
            operation.AddVarcharParam("Mensaje", ex.Message);

            if(ex.StackTrace != null) {
                operation.AddVarcharParam("Pila", ex.StackTrace);
            } else {
                operation.AddVarcharParam("Pila", "No hay pila");
            }

            SqlDao.GetInstance().ExecuteProcedure(operation);
        }

        public ApplicationMessage GetMessage(BussinessException bex)
        {
            var appMessage = new ApplicationMessage();
            appMessage.Message = "Message not found!";

            if (messages.ContainsKey(bex.ExceptionId))
                appMessage = messages[bex.ExceptionId];

            return appMessage;
        }

        private void LoadMessages()
        {
            var crudMessages = new AppMessagesCrudFactory();

            var lstMessages = crudMessages.RetrieveAll<ApplicationMessage>();

            foreach (var appMessage in lstMessages)
            {
                messages.Add(appMessage.Id, appMessage);
            }
        }

        private void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

    }
}
