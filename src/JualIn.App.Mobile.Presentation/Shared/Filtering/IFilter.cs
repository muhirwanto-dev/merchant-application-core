namespace JualIn.App.Mobile.Presentation.Shared.Filtering
{
    public interface IFilter<T>
    {
        string Key { get; }

        ICollection<IFilterSpecification<T>> Specifications { get; }

        IEnumerable<T> Apply(IEnumerable<T> source);
    }
}
