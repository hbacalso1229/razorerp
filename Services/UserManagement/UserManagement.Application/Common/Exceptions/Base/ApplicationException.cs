namespace UserManagement.Application.Common.Exceptions.Base
{
    public abstract class ApplicationException : Exception
    {
        protected ApplicationException(string title, string message)
            : base(message) =>
            Title = title;

        public string Title { get; }

        protected ApplicationException()
           : base()
        {
        }

        protected ApplicationException(string message)
            : base(message)
        {
        }

        protected ApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
