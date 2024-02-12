using FileSenderService.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace FileSenderService.Tests;

public class FileParserTests
{
    [Fact]
    public void FileParser_InvalidDirectory_ShouldThrow()
    {
        var service = new FileParser(new Logger<FileParser>(new LoggerFactory()));

        // ToList required to force enumeration, otherwise it doesnt trigger anything
        service.Invoking(s => s.ParseFiles("$$$$$").ToList())
            .Should()
            .Throw<DirectoryNotFoundException>();
    }
}