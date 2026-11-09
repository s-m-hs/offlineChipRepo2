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
//    public class GrProductsController : ControllerBase
//    {
//        private readonly GrContext _db;
//        private readonly IMapper _mapper;
//        public GrProductsController(GrContext context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _db = context;
//        }

//        // GET: api/GrProducts
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<GProductDTO>>> GetGrProduct()
//        {
//            var resu = await _db.GrProduct.ToListAsync();
//            return Ok(resu.Select(c => _mapper.Map<GProductDTO>(c)).ToList());
//        }

//        // GET: api/GrProducts/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<GProductDTO>> GetGrProduct(int id)
//        {

//            var GrProduct = await _db.GrProduct.FindAsync(id);

//            if (GrProduct == null)
//            {
//                return NotFound();
//            }

//            return _mapper.Map<GProductDTO>(GrProduct);
//        }

//        // PUT: api/GrProducts/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutGrProduct(int id, GProductDTO GProductDTO)
//        {
//            var GrProduct = _mapper.Map<GrProduct>(GProductDTO);
//            if (id != GrProduct.ID)
//            {
//                return BadRequest();
//            }
//            GrProduct.LastModified = DateTime.Now;
//            _db.Entry(GrProduct).State = EntityState.Modified;

//            try
//            {
//                await _db.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!GrProductExists(id))
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

//        // POST: api/GrProducts
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<GProductDTO>> PostGrProduct(GProductDTO GProductDTO)
//        {
//            var GrProduct = _mapper.Map<GrProduct>(GProductDTO);
//            if (_db.GrProduct.Where(c => c.PartNumber == GrProduct.PartNumber).Count() < 1)
//            {
//                GrProduct.CreateDate = DateTime.Now;

//                if (GProductDTO.KeyValues != null && GProductDTO.KeyValues.Count > 0)
//                {
//                    GrProduct.GrKeyValue = GProductDTO.KeyValues.Select(c => _mapper.Map<GrKeyValue>(c)).ToList();
//                }
//                _db.GrProduct.Add(GrProduct);
//                await _db.SaveChangesAsync();

//                return CreatedAtAction("GetGrProduct", new { id = GrProduct.ID }, _mapper.Map<GProductDTO>(GrProduct));
//            }
//            else
//            {
//                return BadRequest("This Item Name is Exists in DB");
//            }

//        }

//        // DELETE: api/GrProducts/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteGrProduct(int id)
//        {
//            var GrProduct = await _db.GrProduct.FindAsync(id);
//            if (GrProduct == null)
//            {
//                return NotFound();
//            }

//            _db.GrProduct.Remove(GrProduct);
//            await _db.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool GrProductExists(int id)
//        {
//            return _db.GrProduct.Any(e => e.ID == id);
//        }
//    }
//}
