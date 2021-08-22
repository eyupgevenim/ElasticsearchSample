using Nest;
using Newtonsoft.Json;
using Search.Elastic.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace Search.Elastic
{
    public class SearchEngine : ISearchEngine
    {
        private readonly IElasticContext _elasticContext;
        private readonly IElasticLowLevelContext _elasticLowLevelContext;
        public SearchEngine(IElasticContext elasticContext, IElasticLowLevelContext elasticLowLevelContext)
        {
            _elasticContext = elasticContext;
            _elasticLowLevelContext = elasticLowLevelContext;
        }

        public List<T> Search<T>(Types.SearchRequest searchRequest) where T : class
        {
            ISearchResponse<T> response = _elasticContext.Search<T>(searchRequest);
            if (response.IsValid)
            {
                return response.Documents.ToList();
            }

            return default(List<T>);
        }

        public string Search(string indexName, string queryJson)
        {
            ISearchResponse<dynamic> response = _elasticContext.SearchRawQuery(indexName, queryJson);
            if (response.IsValid)
            {
                return JsonConvert.SerializeObject(response.Documents);
            }

            return string.Empty;
        }

        public string DoRequest(string method, string path, string data)
        {
            SearchResponse<dynamic> response = _elasticLowLevelContext.DoRequest<SearchResponse<dynamic>>(method, path, data);
            if (response.IsValid)
            {
                return JsonConvert.SerializeObject(response.Hits);
            }

            return string.Empty;
        }
    }
}
