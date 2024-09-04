using UserManagement.Domain.SeedWork;

namespace UserManagement.Domain.Enums
{
    public class RoleType : Enumeration
    {
        public RoleType(string id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// A - Admin role type
        /// </summary>
        public static RoleType Admin => new RoleType("A", nameof(Admin));

        /// <summary>
        /// U - User role type
        /// </summary>
        public static RoleType User => new RoleType("U", nameof(User));

        /// <summary>
        /// List of role types
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<RoleType> List() => new[] { Admin, User };

        public static RoleType FromName(string name)
        {
            RoleType type = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (type == null)
            {
                throw new Exception($"Possible values for RoleType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return type;
        }
    }
}
