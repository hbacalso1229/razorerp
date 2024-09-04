using UserManagement.Domain.Common;
using UserManagement.Domain.SeedWork;

namespace UserManagement.Domain.Entities
{
    public class User : AuditableEntity, IAggregateRoot
    {
        public User()
        {
            Id = Guid.NewGuid();
            LastModified = Created = DateTime.UtcNow;
            CreatedBy = LastModifiedBy = "api";
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Company Id
        /// </summary>
        public Guid CompanyId { get; private set; }

        /// <summary>
        /// Role
        /// </summary>
        public string Role { get; private set; }

        public void SetPassword(string password)
        {
            Password = password;
        }
    }
}
