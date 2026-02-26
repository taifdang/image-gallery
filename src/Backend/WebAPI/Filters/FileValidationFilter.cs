using System.IO;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Filters;

public class FileValidationFilter : IEndpointFilter
{
    private readonly long maxFileSizeBytes = 10485760; // 10MB
    private readonly string[] allowedExtensions = new[] {".jpg",".jpeg",".png",".gif",".webp"};
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.HttpContext.Request;
        var formCollection = await request.ReadFormAsync();
        var files = formCollection.Files;

        if (files.Count == 0) return await next(context);
        foreach(var file in files)
        {
            if(file.Length == 0) throw new ValidationException("File is empty");
            if(file.Length > maxFileSizeBytes) throw new ValidationException("File is too large");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if(allowedExtensions.Contains(fileExtension) == true) continue;
            else throw new ValidationException($"File type {fileExtension} is not allowed") ;
        }

        return await next(context);
    }
}