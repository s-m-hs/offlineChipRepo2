using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CY_DM;
using CY_BM;
using CY_WebApi.Models;
using System.Runtime.InteropServices;
using AutoMapper;
using CY_WebApi.DataAccess;
using CY_WebApi.Services;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyManufacturerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Repository<CyManufacturer> _repo;
        private readonly CyContext _db;
        public CyManufacturerController(CyContext context, IMapper mapper, CyContext db)
        {
            _mapper = mapper;
            _repo = new Repository<CyManufacturer>(context);
            _db = db;
        }

        // GET: api/CyManufacturers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManufacturerDTO>>> GetCyManufacturer()
        {
            var resu = await _db.CyManufacturer.ToListAsync();
            return Ok(resu.Select(c => _mapper.Map<ManufacturerDTO>(c)).ToList());
        }

        [HttpPost("getManufacturersPaged")]
        public async Task<ActionResult<IEnumerable<ManufacturerDTO>>> GetCyManufacturerPaged(PageDTO PD)
        {
            var skipCount = PD.PageNumber * PD.PageSize;

            var allcount = await _db.CyManufacturer.Where(m => m.IsVisible).CountAsync();


            var resu = await _db.CyManufacturer.OrderBy(c => c.Name).Skip(skipCount).Take(PD.PageSize).ToListAsync();
            var pagedres = new PageResultDTO<ManufacturerDTO>
            {

                ItemList = resu.Select(c => _mapper.Map<ManufacturerDTO>(c)).ToList(),
                AllCount = allcount,
            };
            return Ok(pagedres);
        }

        // GET: api/CyManufacturers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ManufacturerDTO>> GetCyManufacturer(Guid id)
        {
            var CyManufacturer = await _repo.Find(id);
            if (CyManufacturer == null || !CyManufacturer.IsVisible)
            {
                return NotFound();
            }

            return _mapper.Map<ManufacturerDTO>(CyManufacturer);
        }

        [HttpGet("getByCode/{code}")]
        public async Task<ActionResult<ManufacturerDTO>> GetCyManufacturerByCode(string code)
        {
            var CyManufacturer = await _repo.TableNoTracking.Where(m => m.IsVisible && m.Code == code).FirstOrDefaultAsync();
            if (CyManufacturer == null)
            {
                return NotFound();
            }
            return _mapper.Map<ManufacturerDTO>(CyManufacturer);
        }

        // PUT: api/CyManufacturers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[TypeAuthorize([UserType.SalesManager, UserType.WarehouseKeeper])]
        public async Task<IActionResult> PutCyManufacturer(Guid id, ManufacturerDTO ManufacturerDTO)
        {
            var CyManufacturer = _mapper.Map<CyManufacturer>(ManufacturerDTO);
            if (id != CyManufacturer.ID)
            {
                return BadRequest("Not Valid ID");
            }
            try
            {
                await _repo.Update(CyManufacturer);//  _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // POST: api/CyManufacturers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[TypeAuthorize([UserType.SalesManager, UserType.WarehouseKeeper])]
        public async Task<ActionResult<ManufacturerDTO>> PostCyManufacturer(ManufacturerDTO ManufacturerDTO)
        {
            var CyManufacturer = _mapper.Map<CyManufacturer>(ManufacturerDTO);
            CyManufacturer = await _repo.Insert(CyManufacturer);
            return CreatedAtAction("GetCyManufacturer", new { id = CyManufacturer.ID }, _mapper.Map<ManufacturerDTO>(CyManufacturer));
        }

        // DELETE: api/CyManufacturers/5
        [HttpDelete("{id}")]
        //[TypeAuthorize([UserType.SalesManager, UserType.WarehouseKeeper])]
        public async Task<IActionResult> DeleteCyManufacturer(Guid id)
        {
            await _repo.Delete(id);
            return NoContent();
        }

        [HttpGet("SearchManufacturer/{searched}")]
        public async Task<ActionResult<List<ManufacturerDTO>>> SearchManufacturer(string searched)
        {
            if (string.IsNullOrEmpty(searched.Trim()))
                return BadRequest("عبارت جست و جو را وارد کنید");
            var manufacturerList = await _repo.TableNoTracking.Where(m => m.Name.Contains(searched)).ToListAsync();
            var res = _mapper.Map<List<ManufacturerDTO>>(manufacturerList);
            return Ok(res);
        }
    }
}
