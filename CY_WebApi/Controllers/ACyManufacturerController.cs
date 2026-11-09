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

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyManufacturerController : ControllerBase
    {
        private readonly CyContext _db;
        private readonly IMapper _mapper;
        public CyManufacturerController(CyContext context, IMapper mapper)
        {
            _mapper = mapper;
            _db = context;
        }

        // GET: api/CyManufacturers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManufacturerDTO>>> GetCyManufacturer()
        {
            var resu = await _db.CyManufacturer.ToListAsync();
            return Ok(resu.Select(c => _mapper.Map<ManufacturerDTO>(c)).ToList());
        }

        // GET: api/CyManufacturers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ManufacturerDTO>> GetCyManufacturer(int id)
        {

            var CyManufacturer = await _db.CyManufacturer.FindAsync(id);

            if (CyManufacturer == null)
            {
                return NotFound();
            }

            return _mapper.Map<ManufacturerDTO>(CyManufacturer);
        }

        // PUT: api/CyManufacturers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCyManufacturer(int id, ManufacturerDTO ManufacturerDTO)
        {
            var CyManufacturer = _mapper.Map<CyManufacturer>(ManufacturerDTO);
            if (id != CyManufacturer.ID)
            {
                return BadRequest();
            }

            _db.Entry(CyManufacturer).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CyManufacturerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CyManufacturers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ManufacturerDTO>> PostCyManufacturer(ManufacturerDTO ManufacturerDTO)
        {
            var CyManufacturer = _mapper.Map<CyManufacturer>(ManufacturerDTO);
            _db.CyManufacturer.Add(CyManufacturer);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetCyManufacturer", new { id = CyManufacturer.ID }, _mapper.Map<ManufacturerDTO>(CyManufacturer));
        }

        // DELETE: api/CyManufacturers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCyManufacturer(int id)
        {
            var CyManufacturer = await _db.CyManufacturer.FindAsync(id);
            if (CyManufacturer == null)
            {
                return NotFound();
            }

            _db.CyManufacturer.Remove(CyManufacturer);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool CyManufacturerExists(int id)
        {
            return _db.CyManufacturer.Any(e => e.ID == id);
        }
    }
}
