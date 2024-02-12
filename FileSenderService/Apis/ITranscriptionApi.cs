using Refit;

namespace FileSenderService.Apis;

public interface ITranscriptionApi
{
    [Multipart]
    [Post("/transcriptions")]
    Task UploadFiles(IEnumerable<FileInfoPart> files);
}