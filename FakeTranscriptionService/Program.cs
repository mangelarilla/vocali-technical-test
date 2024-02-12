var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();

var transcriptionApi = app.MapGroup("/transcriptions");
transcriptionApi.MapPost("/", (IFormFileCollection attachments) =>
{
    var source = builder.Configuration["SourceDir"]!;
    foreach (var attachment in attachments)
    {
        using var stream = new FileStream(Path.Combine(source, attachment.FileName.Replace(".mp3", ".txt")), FileMode.Create);
        attachment.CopyTo(stream);
    }
    
    return Results.Ok();
}).DisableAntiforgery();

app.Run();