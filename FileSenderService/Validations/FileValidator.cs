using FluentValidation;

namespace FileSenderService.Validations;

public class FileValidator : AbstractValidator<FileInfo>
{
    public FileValidator()
    {
        RuleFor(file => file.Length)
            .InclusiveBetween(50_000, 3_000_000)
            .WithMessage("File size must be between 50KB and 3MB");

        RuleFor(file => file.Name)
            .Must(name => name.EndsWith(".mp3", StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("File format must be MP3");
    }
}