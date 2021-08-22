using Nest;
using Search.Elastic.Abstraction;
using Search.Elastic.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Search.Elastic
{
    public class ElasticContext : IElasticContext
    {
        private readonly ElasticClient _elasticClient;

        public ElasticContext(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public ElasticsearchResponse ClusterHealth()
        {
            return new ElasticsearchResponse(_elasticClient.Cluster.Health());
        }

        public bool IndexExists(string indexName)
        {
            return _elasticClient.Indices.Exists(indexName).Exists;
        }

        public bool AliasExists(string aliasName)
        {
            return _elasticClient.Indices.AliasExists(aliasName).Exists;
        }

        public bool AliasExists(string indexName, string aliasName)
        {
            return _elasticClient.Indices.AliasExists(aliasName, s => s.Index(indexName)).Exists;
        }

        public ElasticsearchResponse CreateIndex<T>(string indexName) where T : class
        {
            return new ElasticsearchResponse(_elasticClient.Indices.Create(indexName, index => index.Map<T>(x => x.AutoMap())));
        }

        public ElasticsearchResponse CreateIndex<T>(string indexName, string aliasName) where T : class
        {
            return new ElasticsearchResponse(_elasticClient.Indices.Create(indexName, index => index.Aliases(a => a.Alias(aliasName)).Map<T>(x => x.AutoMap())));
        }

        public ElasticsearchResponse CreateIndex<T>(string indexName, string aliasName, Func<TypeMappingDescriptor<T>, ITypeMapping> selector) where T : class
        {
            return new ElasticsearchResponse(_elasticClient.Indices.Create(indexName, index => index.Aliases(a => a.Alias(aliasName)).Map<T>(selector)));
        }

        public ElasticsearchResponse Index<T>(string indexName, T document) where T : class
        {
            return new ElasticsearchResponse(_elasticClient.Index(document, i => i.Index(indexName)));
        }

        public ElasticsearchResponse IndexBulk<T>(string indexName, IEnumerable<T> documents) where T : class
        {
            //return _elasticClient.IndexMany(documents, indexName);
            return new ElasticsearchResponse(_elasticClient.Bulk(b => b.Index(indexName).IndexMany(documents)));
        }

        public bool DeleteIndex(string indexName)
        {
            if (IndexExists(indexName))
            {
                var deleteIndexResponse = _elasticClient.Indices.Delete(indexName);
                return deleteIndexResponse.IsValid;
            }

            return true;
        }

        public bool PutAlias(string indexName, string aliasName)
        {
            if (!AliasExists(indexName, aliasName))
            {
                var putAliasResponse = _elasticClient.Indices.PutAlias(indexName, aliasName);
                return putAliasResponse.IsValid;
            }

            return true;
        }

        public bool DeleteAlias(string indexName, string aliasName)
        {
            if (AliasExists(indexName, aliasName))
            {
                var deleteAliasResponse = _elasticClient.Indices.DeleteAlias(indexName, aliasName);
                return deleteAliasResponse.IsValid;
            }

            return true;
        }

        public ISearchResponse<T> Search<T>(Types.SearchRequest searchRequest) where T : class
        {
            var descriptor = GetSearchDescriptor<T>(searchRequest.IndexName, searchRequest.Queries, searchRequest.From, searchRequest.Size);

            if (searchRequest.Sort != null)
                descriptor.Sort(s => searchRequest.Sort.Get(s));

            return _elasticClient.Search<T>(descriptor);
        }

        public ISearchResponse<T> SearchRawQuery<T>(string indexName, string rawJson) where T : class
        {
            //return _elasticClient.Search<T>(new SearchRequest { Query = new RawQuery(rawJson) });
            return _elasticClient.Search<T>(s => s.Index(indexName).Query(q => q.Raw(rawJson)));
        }

        public ISearchResponse<dynamic> SearchRawQuery(string indexName, string rawJson)
        {
            return SearchRawQuery<dynamic>(indexName, rawJson);
        }

        private SearchDescriptor<T> GetSearchDescriptor<T>(string indexName, List<Abstraction.IQuery> queries, int from = 0, int size = 10) where T : class
        {
            var descriptor = new SearchDescriptor<T>(indexName);
            descriptor.From(from).Size(size);

            if (queries.Any())
            {
                if (queries.Any(q => q.QueryType == QueryType.Query))
                {
                    var _queries = queries.Where(q => q.QueryType == QueryType.Query).Select(q => q.Query).ToArray();
                    descriptor.Query(q => q.Bool(b => b.Must(_queries)));
                }

                if (queries.Any(q => q.QueryType == QueryType.PostFilter))
                {
                    var filters = queries.Where(q => q.QueryType == QueryType.PostFilter).Select(q => q.Query).ToArray();
                    descriptor.PostFilter(q => q.Bool(b => b.Must(filters)));
                }
            }

            return descriptor;
        }

    }
}
