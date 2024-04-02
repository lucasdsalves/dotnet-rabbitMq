using dotnet_rabbitMq.Base;
using dotnet_rabbitMq.Consumer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IRabbitMqFactory, RabbitMqFactory>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
