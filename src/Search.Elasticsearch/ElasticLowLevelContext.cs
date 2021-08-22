namespace Search.Elastic
{
    using Elasticsearch.Net;
    using Nest;
    using Search.Elastic.Abstraction;
    using System;

    public class ElasticLowLevelContext : IElasticLowLevelContext
    {
        private readonly ElasticClient _elasticClient;

        public ElasticLowLevelContext(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public T DoRequest<T>(string method, string path, string data) where T : class, IElasticsearchResponse, new()
        {
            if (!Enum.TryParse(method.Trim().ToUpper(), out HttpMethod httpMethod))
            {
                throw new ArgumentException(method);
            }

            return _elasticClient.LowLevel.DoRequest<T>(httpMethod, path: path, data: data);
        }

    }
}
