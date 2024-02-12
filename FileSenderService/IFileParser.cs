namespace FileSenderService;

public interface IFileParser
{
    IEnumerable<FileInfo> ParseFiles(string sourcePath);
}