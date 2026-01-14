namespace JualIn.App.Mobile.Presentation.Shared.Filtering
{
    public interface IFilterSpecification<T>
    {
        bool IsSatisfiedBy(T item);
    }
}
