using ES.NET.Client.Demo.Models;
using ES.NET.Client.Demo.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSetting = new AppSetting();
builder.Configuration.GetSection("AppSetting").Bind(appSetting);

//把AppSetting实体注入到容器,方便在构造函数里使用IOptionsSnapshot<AppSetting> options
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

//[FromServices] ESClientConnHelp esClientConnHelp
builder.Services.AddScoped<IESClientConnHelp,ESClientConnHelp>();

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
