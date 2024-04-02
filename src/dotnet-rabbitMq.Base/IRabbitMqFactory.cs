namespace dotnet_rabbitMq.Base
{
    public interface IRabbitMqFactory
    {
        Task PublishMessage(string message);
        Task ConsumeMessage();
    }
}
