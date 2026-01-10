namespace JualIn.Application.Common.Mapping
{
    public interface IMapper
    {
        TResult Map<TSource, TResult>(TSource source);
    }
}
