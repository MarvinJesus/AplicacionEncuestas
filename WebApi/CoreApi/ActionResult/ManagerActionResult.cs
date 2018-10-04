using Exceptions;

namespace CoreApi.ActionResult
{
    public class ManagerActionResult<T> where T : class
    {
        public T Entity { get; set; }
        public ManagerActionStatus Status { get; set; }
        public BussinessException Exception { get; set; }

        public ManagerActionResult(T entity, ManagerActionStatus status)
        {
            Entity = entity;
            Status = status;
        }

        public ManagerActionResult(T entity, ManagerActionStatus status, BussinessException exception) : this(entity, status)
        {
            Exception = exception;
        }
    }
}
