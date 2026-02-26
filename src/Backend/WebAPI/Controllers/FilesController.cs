using Domain.Entities;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class FilesController : Controller
{
   private readonly IFileStorageManager _fileManager;

   public FilesController(IFileStorageManager fileManager)
   {
       _fileManager = fileManager;
   }

   public async Task<IActionResult> Get(string path)
   {
      // Application
      throw new NotImplementedException();
   }

   public async Task<IActionResult> Upload([FromForm] UploadFileModel model)
    {
        // Application

        using var stream = model.FormFile.OpenReadStream();
    
        return Ok();
    }

}
