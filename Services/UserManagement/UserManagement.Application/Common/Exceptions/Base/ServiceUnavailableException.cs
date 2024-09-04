namespace UserManagement.Application.Common.Exceptions.Base
{
    public abstract class ServiceUnavailableException : ApplicationException
    {
        public ServiceUnavailableException(string message)
            : base("Service Unavailable", message)
        {
        }
    }
}
