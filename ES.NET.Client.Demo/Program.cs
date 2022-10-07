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

//��AppSettingʵ��ע�뵽����,�����ڹ��캯����ʹ��IOptionsSnapshot<AppSetting> options
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
