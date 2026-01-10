using System.ComponentModel.DataAnnotations.Schema;

namespace JualIn.Domain.Common.Entities
{
    public abstract class NoKeyEntity : Entity
    {
        [NotMapped]
        public override long Id { get; set; }
    }
}
