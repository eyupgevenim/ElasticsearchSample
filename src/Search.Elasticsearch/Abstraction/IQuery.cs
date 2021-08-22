namespace Search.Elastic.Abstraction
{
    using Nest;
    using Search.Elastic.Types;

    public interface IQuery
    {
        QueryType QueryType { get; }
        QueryContainer Query { get; }
    }
}
