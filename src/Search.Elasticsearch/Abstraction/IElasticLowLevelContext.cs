namespace Search.Elastic.Abstraction
{
    using Elasticsearch.Net;

    public interface IElasticLowLevelContext
    {
        T DoRequest<T>(string method, string path, string data) where T : class, IElasticsearchResponse, new();
    }
}