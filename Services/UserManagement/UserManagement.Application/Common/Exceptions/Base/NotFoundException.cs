namespace UserManagement.Application.Common.Exceptions.Base
{
    public abstract class NotFoundException : ApplicationException
    {
        protected NotFoundException()
        : base()
        {
        }

        protected NotFoundException(string message)
            : base("Not Found", message)
        {
        }

        protected NotFoundException(string title, string message)
        : base(title, message)
        {
        }

        protected NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found")
        {
        }
    }
}
