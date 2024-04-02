using dotnet_rabbitMq.Base;

namespace dotnet_rabbitMq.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMqFactory _rabbitMqFactory;

        public Worker(
            ILogger<Worker> logger,
            IRabbitMqFactory rabbitMqFactory)
        {
            _logger = logger;
            _rabbitMqFactory = rabbitMqFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await _rabbitMqFactory.ConsumeMessage();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
