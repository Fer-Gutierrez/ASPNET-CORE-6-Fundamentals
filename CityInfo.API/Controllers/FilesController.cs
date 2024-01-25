using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{   

    [Route("api/files")]
   [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        //Instanciamos la Extension de ContentTypeProvider agregada como servicio en Program.cs
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        //Creamos el constructor, para que se cree automaticamente una variable de la extension.
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            string pathToFile = "img1.jpeg";

            if (!System.IO.File.Exists(pathToFile)) return NotFound();

            //Aplicamos extension de contentTypeProvider
            if(!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
