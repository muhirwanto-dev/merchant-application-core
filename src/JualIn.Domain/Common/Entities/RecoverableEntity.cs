namespace JualIn.Domain.Common.Entities
{
    public abstract class RecoverableEntity : RecoverableEntity<long>;

    public abstract class RecoverableEntity<TKey> : AuditableEntity<TKey>
        where TKey : notnull, IEquatable<TKey>
    {
        public bool IsDeleted { get; set; } = false;
    }
}
