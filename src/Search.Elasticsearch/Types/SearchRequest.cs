namespace Search.Elastic.Types
{
    using Search.Elastic.Abstraction;
    using System.Collections.Generic;

    public class SearchRequest
    {
        public SearchRequest() : this(new List<IQuery>()) { }

        public SearchRequest(List<IQuery> queries, int from = 0, int size = 10)
        {
            Queries = queries;
            From = from;
            Size = size;
        }

        public string IndexName { get; set; }
        public int From { get; set; }
        public int Size { get; set; }
        public List<IQuery> Queries { get; set; }
        public ISort Sort  { get; set; }
    }
}
