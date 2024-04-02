using dotnet_rabbitMq.Base;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();

services.AddSingleton<IRabbitMqFactory, RabbitMqFactory>();

var provider = services.BuildServiceProvider();

var rabbitMqFactory = provider.GetService<IRabbitMqFactory>();

Console.Write("Enter your message: ");
var message = Console.ReadLine();

if (rabbitMqFactory is not null)
    await rabbitMqFactory.PublishMessage(message);