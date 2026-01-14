using JualIn.App.Mobile.Presentation.Shared.Filtering.Specifications;

namespace JualIn.App.Mobile.Presentation.Shared.Filtering
{
    public class CountableFilter<T> : IFilter<T>
    {
        public string Key { get; }

        public ICollection<IFilterSpecification<T>> Specifications { get; } = [];

        public CountableFilter(Func<T, bool> predicate, string key)
        {
            Specifications.Add(new PredicateFilterSpecification<T>(predicate));
            Key = key;
        }

        public IEnumerable<T> Apply(IEnumerable<T> source)
        {
            return source.Where(item => Specifications.All(spec => spec.IsSatisfiedBy(item)));
        }
    }
}
