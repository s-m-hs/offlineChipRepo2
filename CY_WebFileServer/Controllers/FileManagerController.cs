using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CY_WebFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {


        private readonly IConfiguration _appSetting;

        public FileManagerController(IConfiguration appSetting)
        {
            _appSetting = appSetting;
        }

        [Route("api/File/Upload")]
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var FolderName = Request.Form["FolderName"][0];

                var FileName = Request.Form["FileName"][0];
                if (FileName.ToLower().EndsWith(".exe"))
                    throw new Exception("File can not be exe File.");
                IFormFile file = Request.Form.Files[0];
                if (file == null || file.Length == 0)
                    return Content("file not selected");
                //string baseAdr = Directory.GetCurrentDirectory();
                //baseAdr = Directory.GetParent(baseAdr)!.FullName;
                //string baseFile = Path.Combine(baseAdr, "FileLocation", FolderName);
                //baseAdr = Directory.GetParent(baseAdr)!.Parent!.Parent!.FullName;
                string baseFile = Path.Combine(_appSetting.GetSection("BasePath").Value, FolderName);
                if (!Directory.Exists(baseFile))
                {
                    Directory.CreateDirectory(baseFile);
                }

                if (System.IO.File.Exists(Path.Combine(baseFile, FileName)))
                {
                    throw new Exception("The file name exists in the directory.");
                }
                var path = Path.Combine(
                    baseFile,
                    FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok();
            }
            catch (Exception e)
            {
                //if (e.InnerException != null)
                //{
                //    return BadRequest(e.Message + "-" + e.InnerException!.Message);
                //}
                return BadRequest(e.ToString());
            }
        }

        [Route("api/File/Download")]
        [HttpPost]
        public async Task<IActionResult> Download(Models.FileDTO fileDto)
        {
            try
            {
                //string baseAdr = Directory.GetCurrentDirectory();
                //baseAdr = Directory.GetParent(baseAdr)!.FullName;
                //var path = Path.Combine(baseAdr, "FileLocation", fileDto.FolderName, fileDto.FileName);
                //baseAdr = Directory.GetParent(baseAdr)!.Parent!.Parent!.FullName;
                var path = Path.Combine(_appSetting.GetSection("BasePath").Value, fileDto.FolderName, fileDto.FileName);

                if (!System.IO.File.Exists(path))
                {
                    return BadRequest("file not exist.");
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                //return File(memory, GetContentType(path), Path.GetFileName(path));
                return File(memory, "application/octet-stream");
            }
            catch (Exception e)
            {
                //if (e.InnerException != null)
                //{
                //    return BadRequest(e.Message + "-" + e.InnerException!.Message);
                //}
                return BadRequest(e.ToString());
            }
        }
        //private string GetContentType(string path)
        //{
        //    var types = GetMimeTypes();
        //    var ext = Path.GetExtension(path).ToLowerInvariant();
        //    return types[ext];
        //}

        //private Dictionary<string, string> GetMimeTypes()
        //{
        //    return new Dictionary<string, string>
        //    {
        //        {".txt", "text/plain"},
        //        {".pdf", "application/pdf"},
        //        {".doc", "application/vnd.ms-word"},
        //        {".docx", "application/vnd.ms-word"},
        //        {".xls", "application/vnd.ms-excel"},
        //        {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
        //        {".png", "image/png"},
        //        {".jpg", "image/jpeg"},
        //        {".jpeg", "image/jpeg"},
        //        {".gif", "image/gif"},
        //        {".csv", "text/csv"}
        //    };
        //}
    }

}
