using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class UploadFileModel
{
    [Display(Name = "Name")]
    [StringLength(50, MinimumLength = 0)]
    public string Name { get; set; }

    [Required]
    [Display(Name = "File")]
    public IFormFile FormFile { get; set; }
}
