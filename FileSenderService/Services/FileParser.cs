using FileSenderService.Validations;

namespace FileSenderService.Services;

public class FileParser(ILogger<FileParser> logger) : IFileParser
{
    public IEnumerable<FileInfo> ParseFiles(string sourcePath)
    {
        var validator = new FileValidator();
        foreach (var file in Directory
                     .EnumerateFiles(sourcePath)
                     .Select(f => new FileInfo(f)))
        {
            var result = validator.Validate(file);
            if (result.IsValid)
            {
                yield return file;
            }
            else if (logger.IsEnabled(LogLevel.Debug))
            {
                foreach (var error in result.Errors)
                {
                    logger.LogDebug("File '{File}' invalid: {}", file.Name, error.ErrorMessage);
                }
            }
        }
    }
}