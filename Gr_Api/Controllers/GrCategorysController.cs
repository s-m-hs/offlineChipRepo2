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
//    public class GrCategorysController : ControllerBase
//    {
//        private readonly GrContext _db;
//        private readonly IMapper _mapper;
//        public GrCategorysController(GrContext context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _db = context;
//        }

//        // GET: api/GrCategorys
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<GCategoryDTO>>> GetGrCategory()
//        {
//            var resu = await _db.GrCategory.ToListAsync();
//            return Ok(resu.Select(c => _mapper.Map<GCategoryDTO>(c)).ToList());
//        }

//        // GET: api/GrCategorys/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<GCategoryDTO>> GetGrCategory(int id)
//        {

//            var GrCategory = await _db.GrCategory.FindAsync(id);

//            if (GrCategory == null)
//            {
//                return NotFound();
//            }

//            return _mapper.Map<GCategoryDTO>(GrCategory);
//        }

//        // PUT: api/GrCategorys/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutGrCategory(int id, GCategoryDTO GCategoryDTO)
//        {
//            var GrCategory = _mapper.Map<GrCategory>(GCategoryDTO);
//            if (id != GrCategory.ID)
//            {
//                return BadRequest();
//            }

//            _db.Entry(GrCategory).State = EntityState.Modified;

//            try
//            {
//                await _db.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!GrCategoryExists(id))
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

//        // POST: api/GrCategorys
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<GCategoryDTO>> PostGrCategory(GCategoryDTO GCategoryDTO)
//        {
//            var GrCategory = _mapper.Map<GrCategory>(GCategoryDTO);

//            if (_db.GrCategory.Where(c => c.Text == GrCategory.Text).Count()<1)
//            {

//                _db.GrCategory.Add(GrCategory);
//                await _db.SaveChangesAsync();

//                return CreatedAtAction("GetGrCategory", new { id = GrCategory.ID }, _mapper.Map<GCategoryDTO>(GrCategory));
//            }
//            else
//            {
//                return BadRequest("This Item Name is Exists in DB");
//            }
//        }

//        // DELETE: api/GrCategorys/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteGrCategory(int id)
//        {
//            var GrCategory = await _db.GrCategory.FindAsync(id);
//            if (GrCategory == null)
//            {
//                return NotFound();
//            }

//            _db.GrCategory.Remove(GrCategory);
//            await _db.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool GrCategoryExists(int id)
//        {
//            return _db.GrCategory.Any(e => e.ID == id);
//        }
//    }
//}
