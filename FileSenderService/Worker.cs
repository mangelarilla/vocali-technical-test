using Cronos;
using Microsoft.Extensions.Options;

namespace FileSenderService;

public class Worker : BackgroundService
{
    private readonly WorkerOptions _config;
    private readonly CronExpression _cron;
    private readonly ILogger<Worker> _logger;
    private readonly IFileSender _sender;

    public Worker(ILogger<Worker> logger, IOptions<WorkerOptions> options, IFileSender sender)
    {
        _logger = logger;
        _sender = sender;
        _config = options.Value;
        _cron = CronExpression.Parse(_config.ScheduledAt);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await _sender.SendFiles(_config.SourceDir);
            
            var utcNow = DateTime.UtcNow;
            var nextUtc = _cron.GetNextOccurrence(utcNow);
            await Task.Delay(nextUtc!.Value - utcNow, stoppingToken);
        }
    }
}

public record WorkerOptions
{
    public string ScheduledAt { get; init; }
    public string SourceDir { get; init; }
}