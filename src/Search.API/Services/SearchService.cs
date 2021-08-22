using Microsoft.Extensions.Options;
using Search.API.Models;
using Search.Elastic.Abstraction;
using Search.Elastic.Queries;
using Search.Elastic.Sorts;
using Search.Elastic.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Search.API.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOptions<ElasticsearchSettings> _elasticsearchAppsettings;
        private readonly ISearchEngine _searchEngine;

        public SearchService(IOptions<ElasticsearchSettings> elasticsearchAppsettings, ISearchEngine searchEngine)
        {
            _elasticsearchAppsettings = elasticsearchAppsettings ?? throw new ArgumentNullException(nameof(elasticsearchAppsettings));
            _searchEngine = searchEngine ?? throw new ArgumentNullException(nameof(searchEngine));
        }

        public List<ProductDto> Search(FilterOption filterOption)
        {
            var searchRequest = new SearchRequest { IndexName = _elasticsearchAppsettings.Value.AliasName, From = filterOption.From, Size = filterOption.Size };

            if (!string.IsNullOrEmpty(filterOption.Name))
            {
                searchRequest.Queries.Add(new ProductNameQuery { Value = filterOption.Name });
            }

            if (filterOption.Prices != (0, double.MaxValue))
            {
                searchRequest.Queries.Add(new ProductPriceQuery { MinValue = filterOption.Prices.Min, MaxValue = filterOption.Prices.Max });
            }

            if (filterOption.Sorts.Any())
            {
                var sort = new ProductSort();

                if (filterOption.SortByName != null)
                    sort.OrderByName(filterOption.SortByName.Value);

                if (filterOption.SortByPrice != null)
                    sort.OrderByPice(filterOption.SortByPrice.Value);

                searchRequest.Sort = sort;
            }

            return _searchEngine.Search<ProductDto>(searchRequest);
        }

        public string Search(string queryJson) => _searchEngine.Search(_elasticsearchAppsettings.Value.AliasName, queryJson);
        public string DoRequest(string method, string path, string data) => _searchEngine.DoRequest(method, path, data);

    }
}
