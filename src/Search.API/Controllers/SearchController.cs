using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Search.API.Models;
using Search.API.Services;
using System;

namespace Search.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchService _searchService;

        public SearchController(ILogger<SearchController> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        [HttpGet]
        public virtual ActionResult Get() => RunSafely(() => Ok(_searchService.Search(new FilterOption())));

        [HttpPost]
        public virtual ActionResult Post(FilterOption filterOption) => RunSafely(() => Ok(_searchService.Search(filterOption)));

        [HttpPost("QueryJson")]
        public virtual ActionResult QueryJson(string queryJson) => RunSafely(() => Ok(_searchService.Search(queryJson)));

        [HttpPost("DoRequest")]
        public virtual ActionResult DoRequest(DoRequest request) => RunSafely(() => Ok(_searchService.DoRequest(request.Method, request.Path, request.Data)));

        private ActionResult RunSafely(Func<ActionResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
