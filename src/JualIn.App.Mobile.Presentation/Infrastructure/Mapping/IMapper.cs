namespace JualIn.App.Mobile.Presentation.Infrastructure.Mapping
{
    public interface IMapper
    {
        TResult Map<TSource, TResult>(TSource source);

        void MapTo<TSource, TResult>(TSource source, TResult result);
    }
}
