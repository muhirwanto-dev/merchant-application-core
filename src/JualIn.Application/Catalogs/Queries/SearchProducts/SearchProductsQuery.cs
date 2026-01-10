using Wolverine;

namespace JualIn.Application.Catalogs.Queries.GetProducts
{
    public record SearchProductsQuery(string Query) : IMessage;
}
