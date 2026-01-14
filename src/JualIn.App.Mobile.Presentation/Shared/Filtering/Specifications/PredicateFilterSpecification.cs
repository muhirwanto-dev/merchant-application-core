namespace JualIn.App.Mobile.Presentation.Shared.Filtering.Specifications
{
    public class PredicateFilterSpecification<T>(Func<T, bool> _predicate) : IFilterSpecification<T>
    {
        public bool IsSatisfiedBy(T item) => _predicate(item);
    }
}
