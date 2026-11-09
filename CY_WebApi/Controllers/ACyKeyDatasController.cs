using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CY_DM;
using CY_WebApi.Models;
using CY_BM;
using AutoMapper;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyKeyDatasController : ControllerBase
    {
        private readonly CyContext _db;
        private readonly IMapper _mapper;

        public CyKeyDatasController(CyContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        // GET: api/CyKeyDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KeyDataDTO>>> GetCyKeyData()
        {
            var resu = await _db.CyKeyData.ToListAsync();
            return Ok(resu.Select(c => _mapper.Map<KeyDataDTO>(c)).ToList());
        }

        // GET: api/CyKeyDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KeyDataDTO>> GetCyKeyData(int id)
        {

            var cyKeyData = await _db.CyKeyData.FindAsync(id);

            if (cyKeyData == null)
            {
                return NotFound();
            }

            return _mapper.Map<KeyDataDTO>(cyKeyData);
        }

        // PUT: api/CyKeyDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCyKeyData(int id, KeyDataDTO KeyData)
        {
            var cyKeyData=_mapper.Map<CyKeyData>(KeyData);
            if (id != cyKeyData.ID)
            {
                return BadRequest();
            }

            _db.Entry(cyKeyData).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CyKeyDataExists(id))
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

        // POST: api/CyKeyDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KeyDataDTO>> PostCyKeyData(KeyDataDTO KeyData)
        {
            var cyKeyData = _mapper.Map<CyKeyData>(KeyData);
            _db.CyKeyData.Add(cyKeyData);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetCyKeyData", new { id = cyKeyData.ID }, _mapper.Map<KeyDataDTO>(cyKeyData));
        }

        // DELETE: api/CyKeyDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCyKeyData(int id)
        {
            var cyKeyData = await _db.CyKeyData.FindAsync(id);
            if (cyKeyData == null)
            {
                return NotFound();
            }

            _db.CyKeyData.Remove(cyKeyData);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool CyKeyDataExists(int id)
        {
            return _db.CyKeyData.Any(e => e.ID == id);
        }
    }
}
