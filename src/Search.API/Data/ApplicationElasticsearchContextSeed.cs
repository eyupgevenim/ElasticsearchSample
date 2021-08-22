namespace Search.API.Data
{
    using Microsoft.Extensions.Logging;
    using Search.Elastic;
    using Search.Elastic.Abstraction;
    using Search.Elastic.Models;
    using Search.Elastic.Types;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ApplicationElasticsearchContextSeed
    {
        public async Task SeedAsync(ElasticsearchSettings elasticsearchSettings, IElasticContext context, ILogger<IElasticContext> logger, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                string aliasName = elasticsearchSettings.AliasName;
                if (!context.AliasExists(aliasName))
                {
                    var indexName = $"product_{DateTime.UtcNow.Ticks}";

                    var mapperResponse = context.ProductMapper(indexName, aliasName);
                    mapperResponse.EnsureSuccess();

                    var bulkResponse = context.IndexBulk(indexName, ProductDtos);
                    bulkResponse.EnsureSuccess();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;
                    logger.LogError(ex, "EXCEPTION ERROR while migrating {ElasticContextName}", nameof(IElasticContext));
                    await SeedAsync(elasticsearchSettings, context, logger, retryForAvaiability);
                }
            }
        }

        private IEnumerable<Product> ProductDtos => new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Test Name",
                Description = "Test Description",
                Price = 64M,
                Quantity = 30,
                Tags = new List<string>{ "test", "test 1" },
                ProductProperties = new List<ProductProperty>
                {
                    new ProductProperty{ Key = "key1", Value = "value1" }
                }
            },
            new Product
            {
                Id = 2,
                Name = "Test Name 2",
                Description = "Test Description 2",
                Price = 85M,
                Quantity = 4,
                Tags = new List<string>{ "test2", "test22" },
                ProductProperties = new List<ProductProperty>
                {
                    new ProductProperty{ Key = "key2", Value = "value2" }
                }
            },
            new Product
            {
                Id = 3,
                Name = "Test Name 3",
                Description = "Test Description 3",
                Price = 26M,
                Quantity = 12,
                Tags = new List<string>{ "test3", "test33" },
                ProductProperties = new List<ProductProperty>
                {
                    new ProductProperty{ Key = "key3", Value = "value3" }
                }
            }

        };

    }
}
