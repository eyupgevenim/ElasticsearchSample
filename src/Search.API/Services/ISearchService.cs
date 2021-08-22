namespace Search.API.Services
{
    using Search.API.Models;
    using System.Collections.Generic;

    public interface ISearchService
    {
        List<ProductDto> Search(FilterOption filterOption);
        string Search(string queryJson);
        string DoRequest(string method, string path, string data);
    }
}
