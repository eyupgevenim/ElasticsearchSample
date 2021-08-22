namespace Search.Elastic.Queries
{
    using Nest;
    using Search.Elastic.Models;
    using Search.Elastic.Types;

    public class ProductPriceQuery : QueryContainerDescriptor<Product>, Abstraction.IQuery
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public QueryType QueryType => QueryType.PostFilter;
        public QueryContainer Query => Range(r => r.Field(f => f.Price).GreaterThanOrEquals(MinValue).LessThanOrEquals(MaxValue));
    }
}
