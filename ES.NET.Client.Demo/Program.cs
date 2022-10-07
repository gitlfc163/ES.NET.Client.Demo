using ES.NET.Client.Demo.Models;
using ES.NET.Client.Demo.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API标题",
        Description = "API描述"
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var appSetting = new AppSetting();
builder.Configuration.GetSection("AppSetting").Bind(appSetting);

//把AppSetting实体注入到容器,方便在构造函数里使用IOptionsSnapshot<AppSetting> options
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

//[FromServices] ESClientConnHelp esClientConnHelp
builder.Services.AddScoped<IElasticSearchHelper,ElasticSearchHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
