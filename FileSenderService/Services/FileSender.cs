using Microsoft.Extensions.Options;

namespace FileSenderService.Services;

internal class FileSender(
    IFileParser parser,
    IExternalSender externalSender,
    IOptions<FileSenderOptions> options,
    ILogger<FileSender> logger)
    : IFileSender
{
    private readonly FileSenderOptions _options = options.Value;

    public async Task SendFiles(string source)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Sending files in blocks of {FileLimit} concurrently in blocks of {MaxConcurrent}", 
                _options.FileLimit, _options.MaxConcurrentSends);
        }
        
        var validFiles = parser.ParseFiles(source).ToList();
        
        var fileBlocks = SplitInBlocks(validFiles, _options.FileLimit);
        var maxConcurrentBlocks = SplitInBlocks(fileBlocks.ToList(), _options.MaxConcurrentSends);

        foreach (var concurrentBlock in maxConcurrentBlocks)
        {
            var sends = concurrentBlock.Select(block => externalSender.Send(block));
            await Task.WhenAll(sends);
        }
    }

    // there are more performant ways of doing this for a high volume of files like https://stackoverflow.com/a/13710023
    // but this is more readable for this demo case
    public IEnumerable<IEnumerable<T>> SplitInBlocks<T>(ICollection<T> files, int block)
    {
        var total = files.Count / block + files.Count % block;
        for (var i = 0; i < total; i++)
        {
            yield return files
                .Skip(block * i)
                .Take(block);
        }
    }
}

public record FileSenderOptions
{
    public int FileLimit { get; init; }
    public int MaxConcurrentSends { get; init; }
}