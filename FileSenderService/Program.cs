using FileSenderService;
using FileSenderService.Apis;
using FileSenderService.Services;
using Polly;
using Polly.Extensions.Http;
using Refit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<WorkerOptions>(builder.Configuration);
builder.Services.Configure<FileSenderOptions>(builder.Configuration.GetSection("FileSender"));

builder.Services.AddTransient<IFileParser, FileParser>();
builder.Services.AddTransient<IFileSender, FileSender>();
builder.Services.AddTransient<IExternalSender, TranscriptionService>();

builder.Services.AddRefitClient<ITranscriptionApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["TranscriptionApi"]!))
    .AddPolicyHandler(GetRetryPolicy());

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
            retryAttempt)));
}
    
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

