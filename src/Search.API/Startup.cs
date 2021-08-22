namespace Search.API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Search.API.Services;
    using Search.Elastic;
    using Search.Elastic.Types;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Search.API", Version = "v1" });
            });
            services.AddCors();

            services.AddScoped<ISearchService, SearchService>();

            services
                .Configure<AppSettings>(Configuration)
                .Configure<ElasticsearchSettings>(Configuration.GetSection(nameof(AppSettings.ElasticsearchSettings)));

            var elasticsearchSettings = Configuration.GetSection(nameof(AppSettings.ElasticsearchSettings)).Get<ElasticsearchSettings>();
            services.AddConfigureElasticsearch(elasticsearchSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search.API v1"));

            }

            // Enable CORS so the Vue client can send requests
            app.UseCors(builder =>
            {
                var origins = Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
                builder
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins(origins)
                    //.SetIsOriginAllowed((host) => true) //todo:?
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
