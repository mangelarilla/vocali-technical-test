namespace FileSenderService;

public interface IExternalSender
{
    public Task Send(IEnumerable<FileInfo> files);
}