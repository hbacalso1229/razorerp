using UserManagement.Domain.Enums;
using Dapper;
using System.Data;

namespace UserManagement.Infrastructure.Common.Handlers
{
    public class RoleTypeHandler : SqlMapper.TypeHandler<RoleType>
    {
        public override void SetValue(IDbDataParameter parameter, RoleType role)
        {
            parameter.Value = role;
        }

        public override RoleType Parse(object value)
        {
            return RoleType.FromName(value.ToString());
        }
    }
}
