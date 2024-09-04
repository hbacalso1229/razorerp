using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Domain.SeedWork
{
    public abstract class Entity
    {
        public virtual Guid Id { get; protected set; }        
    }
}
