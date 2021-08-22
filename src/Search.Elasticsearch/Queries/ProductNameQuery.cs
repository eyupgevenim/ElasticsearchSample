namespace Search.Elastic.Queries
{
    using Nest;
    using Search.Elastic.Models;
    using Search.Elastic.Types;

    public class ProductNameQuery : QueryContainerDescriptor<Product>, Abstraction.IQuery
    {
        public string Value { get; set; }
        public QueryType QueryType => QueryType.Query;
        public QueryContainer Query =>
            //QueryString(s => s.Fields(f => f.Field(ff => ff.Name)).DefaultOperator(op:Operator.And).Query(Value));
            //Term(t => t.Field(f => f.Name).Value(Value)); 
            Match(m => m.Field(f => f.Name).Operator(op: Operator.And).Query(Value));
    }
}
