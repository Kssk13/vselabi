using Contracts;
using Entities.DataTransferObjects;
using LR_WEB_API.ActionFilters;
using LR_WEB_API.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using Repository.DataShaping;
//using ShopApi.Extensions;

namespace ShopApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
            "/nlog.config"));
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureVersioning();
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddScoped<IDataShaper<OrderDto>, DataShaper<OrderDto>>();
        services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
        services.AddScoped<ValidateWarehouseExistsAttribute>();
        services.AddScoped<ValidateEmployeeForCompanyExistsAttribute>();
        services.AddScoped<ValidateCompanyExistsAttribute>();
        services.AddScoped<ValidationFilterAttribute>();
        services.Configure<ApiBehaviorOptions>(options =>{options.SuppressModelStateInvalidFilter = true;});
        services.ConfigureCors();
        services.ConfigureIISIntegration();
        services.ConfigureLoggerService();
        services.ConfigureSqlContext(Configuration);
        services.ConfigureRepositoryManager();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(typeof(Startup));
        services.AddControllers(config =>{config.RespectBrowserAcceptHeader = true; config.ReturnHttpNotAcceptable = true;})

        .AddXmlDataContractSerializerFormatters()
        .AddCustomCSVFormatter()
        .AddNewtonsoftJson();


    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
    ILoggerManager logger)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.ConfigureExceptionHandler(logger);
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("CorsPolicy");
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseHsts();

        app.UseStaticFiles();

        app.UseCors("CorsPolicy");

        app.UseForwardedHeaders(new ForwardedHeadersOptions

        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
