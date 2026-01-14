namespace JualIn.App.Mobile.Presentation.Shared.Filtering
{
    public interface IFilterState<T>
    {
        IFilter<T> Model { get; }

        void UpdateState(IEnumerable<T> filtered);
    }
}
