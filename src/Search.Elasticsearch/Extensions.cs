using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Search.Elastic.Abstraction;
using Search.Elastic.Models;
using Search.Elastic.Types;
using System;
using System.Linq;

namespace Search.Elastic
{
    public static class ConfigureExtension
    {
        public static IServiceCollection AddConfigureElasticsearch(this IServiceCollection services, ElasticsearchSettings elasticsearchSettings)
        {
            services.AddScoped<IElasticContext>(sp =>
            {
                var connectionPool = new SingleNodeConnectionPool(elasticsearchSettings.Uris.FirstOrDefault());//new SniffingConnectionPool(elasticsearchSettings.Uris)
                var settings = new ConnectionSettings(connectionPool).DefaultIndex("product");
                settings.RequestTimeout(TimeSpan.FromHours(3));
                var elasticClient = new ElasticClient(settings);

                //OnRequestCompleted(settings);

                return new ElasticContext(elasticClient);
            });

            services.AddScoped<IElasticLowLevelContext>(sp =>
            {
                var connectionPool = new SingleNodeConnectionPool(elasticsearchSettings.Uris.FirstOrDefault());
                var settings = new ConnectionSettings(connectionPool).DefaultIndex("product");
                settings.RequestTimeout(TimeSpan.FromHours(3));
                var elasticClient = new ElasticClient(settings);

                return new ElasticLowLevelContext(elasticClient);
            });

            services.AddScoped<ISearchEngine, SearchEngine>();

            return services;
        }

        static void OnRequestCompleted(ConnectionSettings settings)
        {
            settings.DisableDirectStreaming(true);
            settings.OnRequestCompleted(call =>
            {
                var request = System.Text.Encoding.UTF8.GetString(call.RequestBodyInBytes);
                Console.WriteLine($"request:{request}");

                var response = System.Text.Encoding.UTF8.GetString(call.ResponseBodyInBytes);
                Console.WriteLine($"response:{response}");

            });
        }
    }

    public static class MapperExtension
    {
        public static ElasticsearchResponse ProductMapper(this IElasticContext context, string indexName, string aliasName)
        {
            return context.CreateIndex<Product>(indexName,
                aliasName,
                m => m.AutoMap().Properties(p =>
                    p.Text(t =>
                        t.Name(n => n.Name)
                        .Fielddata(true)
                    )
                ));
        }
    }
}
