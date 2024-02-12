namespace FileSenderService;

public interface IFileSender
{
    public Task SendFiles(string source);
}

