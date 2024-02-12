using FileSenderService.Apis;
using Refit;

namespace FileSenderService.Services;

public class TranscriptionService(ITranscriptionApi transcriptionApi, ILogger<TranscriptionService> logger) : IExternalSender
{
    public Task Send(IEnumerable<FileInfo> files)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Sending files to the service");
        }
        return transcriptionApi.UploadFiles(files.Select(file => new FileInfoPart(file, file.Name)));
    }
}