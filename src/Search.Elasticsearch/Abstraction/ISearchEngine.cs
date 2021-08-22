namespace Search.Elastic.Abstraction
{
    using Search.Elastic.Types;
    using System.Collections.Generic;

    public interface ISearchEngine
    {
        List<T> Search<T>(SearchRequest searchRequest) where T : class;
        string Search(string indexName, string queryJson);
        string DoRequest(string method, string path, string data);
    }
}
