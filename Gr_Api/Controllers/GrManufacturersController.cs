//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CY_DM;
//using CY_BM;

//using System.Runtime.InteropServices;
//using AutoMapper;
//using Gr_Api.Models;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GrManufacturersController : ControllerBase
//    {
//        private readonly GrContext _db;
//        private readonly IMapper _mapper;
//        public GrManufacturersController(GrContext context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _db = context;
//        }

//        // GET: api/GrManufacturers
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<GManufacturerDTO>>> GetGrManufacturer()
//        {
//            var resu = await _db.GrManufacturer.ToListAsync();
//            return Ok(resu.Select(c => _mapper.Map<GManufacturerDTO>(c)).ToList());
//        }

//        // GET: api/GrManufacturers/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<GManufacturerDTO>> GetGrManufacturer(int id)
//        {

//            var GrManufacturer = await _db.GrManufacturer.FindAsync(id);

//            if (GrManufacturer == null)
//            {
//                return NotFound();
//            }

//            return _mapper.Map<GManufacturerDTO>(GrManufacturer);
//        }

//        // PUT: api/GrManufacturers/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutGrManufacturer(int id, GManufacturerDTO GManufacturerDTO)
//        {
//            var GrManufacturer = _mapper.Map<GrManufacturer>(GManufacturerDTO);
//            if (id != GrManufacturer.ID)
//            {
//                return BadRequest();
//            }

//            _db.Entry(GrManufacturer).State = EntityState.Modified;

//            try
//            {
//                await _db.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!GrManufacturerExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/GrManufacturers
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<GManufacturerDTO>> PostGrManufacturer(GManufacturerDTO GManufacturerDTO)
//        {
//            var GrManufacturer = _mapper.Map<GrManufacturer>(GManufacturerDTO);

//            if (_db.GrManufacturer.Where(c => c.Name == GrManufacturer.Name).Count() < 1)
//            {

//                _db.GrManufacturer.Add(GrManufacturer);
//                await _db.SaveChangesAsync();

//                return CreatedAtAction("GetGrManufacturer", new { id = GrManufacturer.ID }, _mapper.Map<GManufacturerDTO>(GrManufacturer));
//            }
//            else
//            {
//                return BadRequest("This Item Name is Exists in DB");
//            }
//        }

//        // DELETE: api/GrManufacturers/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteGrManufacturer(int id)
//        {
//            var GrManufacturer = await _db.GrManufacturer.FindAsync(id);
//            if (GrManufacturer == null)
//            {
//                return NotFound();
//            }

//            _db.GrManufacturer.Remove(GrManufacturer);
//            await _db.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool GrManufacturerExists(int id)
//        {
//            return _db.GrManufacturer.Any(e => e.ID == id);
//        }
//    }
//}
