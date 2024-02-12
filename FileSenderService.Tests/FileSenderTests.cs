using FileSenderService.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace FileSenderService.Tests;

public class FileSenderTests
{
    [Fact]
    public void FileSender_SplitInBlocks_Should_DivideInSpecifiedBlocks()
    {
        var data = Enumerable.Range(1, 10).ToList();
        
        var service = new FileSender(
            Substitute.For<IFileParser>(),
            Substitute.For<IExternalSender>(),
            Substitute.For<IOptions<FileSenderOptions>>(),
            null!);

        var result = service.SplitInBlocks(data, 3);

        result.Should().HaveCount(4);
    }
}