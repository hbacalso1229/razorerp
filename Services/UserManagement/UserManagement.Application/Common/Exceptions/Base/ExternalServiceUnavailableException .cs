namespace UserManagement.Application.Common.Exceptions.Base
{
    public class ExternalServiceUnavailableException : ServiceUnavailableException
    {
        public ExternalServiceUnavailableException(string message, string errormessage = "")
            : base($"The Service is temporarily unavailable. '{message}'. Please contact your system administrator for more information.")
        {
        }
    }
}
