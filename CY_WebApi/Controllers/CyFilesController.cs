using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CY_DM;
using CY_WebApi.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using CY_BM;
using CY_WebApi.DataAccess;
using AutoMapper;
using static Grpc.Core.Metadata;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyFilesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CyContext _db;
        private readonly Repository<CyFile> _repo;
        public CyFilesController(CyContext context, IMapper mapper)
        {
            _repo = new Repository<CyFile>(context);
            _mapper = mapper;
            _db = context;
        }

        // GET: api/CyFiles
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CyFile>>> GetCyFile()
        //{
        //    return await _context.CyFile.ToListAsync();
        //}

        //// GET: api/CyFiles/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<CyFile>> GetCyFile(int id)
        //{
        //    var cyFile = await _context.CyFile.FindAsync(id);

        //    if (cyFile == null)
        //    {
        //        return NotFound();
        //    }

        //    return cyFile;
        //}

        //// PUT: api/CyFiles/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCyFile(int id, CyFile cyFile)
        //{
        //    if (id != cyFile.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(cyFile).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CyFileExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<ActionResult<OutFileModel>> PostCyFile([FromForm] FileUploadModel model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("InValid File");
            }
            var BaseFolderPath = Path.Combine("FileContents", model.IsPrivate ? "private" : "public");
            var timeBaseFolder = getTimeBaseFolderName(DateTime.Now);
            var RelatedAddress = Path.Combine(BaseFolderPath, timeBaseFolder);
            var FinalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), RelatedAddress);
            if (!Directory.Exists(FinalFolderPath))
            {
                Directory.CreateDirectory(FinalFolderPath);
            }

            var fileName = string.IsNullOrWhiteSpace(model.Name) ? model.File.FileName : model.Name;
            var guidFileIQ = Guid.NewGuid();
            long longFs = 0;
            try
            {
                longFs = model.File.Length;
            }
            catch (Exception ex)
            {
            }
            var filedto = new CyFile()
            {
                FileName = fileName,
                FileType = Path.GetExtension(fileName),
                FolderName = timeBaseFolder,
                CreateDate = DateTime.Now,
                FileIQ = guidFileIQ,
                FileSize = longFs,
                IsVisible = true

            };
            if (model.IsPrivate)
            {
                fileName = filedto.FileIQ + filedto.FileType;
            }
            var fullPath = Path.Combine(FinalFolderPath, fileName);
            //if (System.IO.File.Exists(fullPath))
            //{
            while (System.IO.File.Exists(fullPath))
            {
                fileName = "new" + fileName;
                fullPath = Path.Combine(FinalFolderPath, fileName);
            }
            //return BadRequest("File Already Exist!!");
            // }

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }
            if (model.IsPrivate)
            {
                //await _repo.Insert(filedto);

                if (filedto == null)
                    throw new ArgumentNullException("entity");
                if (filedto.ID == null || filedto.ID == Guid.Empty)
                    filedto.ID = Guid.NewGuid();
                var tt = _db.CyFile.Add(filedto);
                filedto.CreateDate = DateTime.Now;
                filedto.IsVisible = true;
                //  entity = _db.Set<T>().Add(entity);
                await _db.SaveChangesAsync();
                //}
                //catch (DbEntityValidationException dbEx)
                //{
                //    throw new Exception(dbEx.Message, dbEx);
                //}
                //return tt.Entity;




                return Ok(new OutFileModel() { ID = filedto.FileIQ });
            }
            else
            {
                var url = Path.Combine("GFiles", getTimeBaseFolderName(DateTime.Now), fileName);
                url = url.Replace("\\", "/");
                var ofm = new OutFileModel() { Adress = url };
                return Ok(ofm);
            }

        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadById(Guid id)
        {
            var selFile = _repo.FirstOrDefault(c => c.FileIQ == id);
            if (selFile != null)
            {
                var BaseFolderPath = Path.Combine("FileContents", "private", selFile.FolderName);
                var RelatedAddress = Path.Combine(BaseFolderPath, selFile.FileIQ + selFile.FileType);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), RelatedAddress);

                if (!System.IO.File.Exists(fullPath))
                {
                    return BadRequest("File not Exist!!");
                }

                Response.Headers.Add("Content-Disposition", $"attachment; filename={selFile.FileName}");

                var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);

                var fileContentResult = new FileContentResult(fileBytes, "application/octet-stream")
                {
                    FileDownloadName = selFile.FileName,

                };
                return fileContentResult;
            }
            else
            {
                return NotFound();
            }
        }
        // POST: api/CyFiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<CyFile>> PostCyFile(CyFile cyFile)
        //{
        //    _context.CyFile.Add(cyFile);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCyFile", new { id = cyFile.ID }, cyFile);
        //}

        //// DELETE: api/CyFiles/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCyFile(int id)
        //{
        //    var cyFile = await _context.CyFile.FindAsync(id);
        //    if (cyFile == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.CyFile.Remove(cyFile);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        public static string getTimeBaseFolderName(DateTime dt)
        {

            var yy = dt.Year - 2000;
            var mm = dt.Month + "";
            var dd = dt.Day + "";
            if (mm.Length < 2)
            {
                mm = "0" + mm;
            }
            if (dd.Length < 2)
            {
                dd = "0" + dd;
            }
            return yy + "-" + mm + "-" + dd;
        }

    }
}
