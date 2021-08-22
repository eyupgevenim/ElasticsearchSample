namespace Search.Elastic.Abstraction
{
    using Nest;
    using Search.Elastic.Types;
    using System;
    using System.Collections.Generic;

    public interface IElasticContext
    {
        ElasticsearchResponse ClusterHealth();
        bool IndexExists(string indexName);
        bool AliasExists(string aliasName);
        bool AliasExists(string indexName, string aliasName);
        ElasticsearchResponse CreateIndex<T>(string indexName) where T : class;
        ElasticsearchResponse CreateIndex<T>(string indexName, string aliasName) where T : class;
        ElasticsearchResponse CreateIndex<T>(string indexName, string aliasName, Func<TypeMappingDescriptor<T>, ITypeMapping> selector) where T : class;
        ElasticsearchResponse Index<T>(string indexName, T document) where T : class;
        ElasticsearchResponse IndexBulk<T>(string indexName, IEnumerable<T> documents) where T : class;
        bool DeleteIndex(string indexName);
        bool PutAlias(string indexName, string aliasName);
        bool DeleteAlias(string indexName, string aliasName);

        ISearchResponse<T> Search<T>(Types.SearchRequest searchRequest) where T : class;
        ISearchResponse<T> SearchRawQuery<T>(string indexName, string rawJson) where T : class;
        ISearchResponse<dynamic> SearchRawQuery(string indexName, string rawJson);
    }
}
