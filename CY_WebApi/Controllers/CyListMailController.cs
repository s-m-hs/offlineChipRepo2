//using CY_BM;
//using CY_DM;
//using CY_WebApi.DataAccess;
//using CY_WebApi.Models;
//using CY_WebApi.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;
//using System.Text;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CyListMailController : ControllerBase
//    {
//        private readonly Repository<CyListMail> _repo;

//        public CyListMailController(CyContext db)
//        {
//            _repo = new Repository<CyListMail>(db);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddEmail(string Email)
//        {
//            var newEmail = new CyListMail() { Mail = Email };
//            return Ok(await _repo.Insert(newEmail));
//        }

//        //[TypeAuthorize([UserType.SysAdmin, UserType.WarehouseKeeper])]
//        [HttpGet]
//        public async Task<IActionResult> GetAllEmails()
//        {
//            //var identity = User.Identities.FirstOrDefault();
//            //if (identity == null)
//            //    return Unauthorized("کاربر پیدا نشد");

//            var initialRes = await _repo.TableNoTracking.Where(e => e.IsVisible).Select(e => e.Mail).ToListAsync();

//            // Create a memory stream to hold the CSV data
//            using (var memoryStream = new MemoryStream())
//            {
//                // Create a StreamWriter to write the CSV data to the memory stream
//                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, bufferSize: 1024, leaveOpen: true))
//                {
//                    foreach (string mail in initialRes)
//                    {
//                        writer.WriteLine(mail);
//                    }
//                }
//                // Reset the memory stream position to the beginning
//                memoryStream.Position = 0;
//                // Read the memory stream to get the file bytes
//                var fileBytes = memoryStream.ToArray();


//                var fileContentResult = new FileContentResult(fileBytes, "application/octet-stream")
//                {
//                    FileDownloadName = "emails.csv",

//                };
//                return fileContentResult;
//            }
//        }
//    }
//}
