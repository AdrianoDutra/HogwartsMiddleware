using Hogwarts.Middleware.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Hogwarts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var circuitBreakerPolicy = Policy
                    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30), OnBreak, OnReset, OnHalfOpen);


            ConfigurationDependency.ConfigureDependencies(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hogwarts", Version = "v1" });
            });

            services.AddHttpClient("PotterApiService", client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("PotterApi"));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddPolicyHandler(circuitBreakerPolicy)
              .AddPolicyHandler(GetRetryPolicy());


            services.AddHttpClient("HogwartsRepositoryApiService", client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("HogwartsRepository"));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddPolicyHandler(circuitBreakerPolicy)
              .AddPolicyHandler(GetRetryPolicy()); 

            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hogwarts v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void OnHalfOpen()
        {
            Console.WriteLine("Circuit in test mode, one request will be allowed.");
        }

        private void OnReset()
        {
            Console.WriteLine("Circuit closed, requests flow normally.");
        }

        private void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan ts)
        {
            Console.WriteLine("Circuit cut, requests will not flow.");
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                // HttpRequestException, 5XX and 408  
                .HandleTransientHttpError()
                // 404  
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                // Retry two times after delay  
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                ;
        }
    }
}
