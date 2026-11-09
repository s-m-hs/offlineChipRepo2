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
    public class CyProductsController : ControllerBase
    {
        private readonly CyContext _db;
        private readonly IMapper _mapper;
        public CyProductsController(CyContext context, IMapper mapper)
        {
            _mapper = mapper;
            _db = context;
        }

        // GET: api/CyProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetCyProduct()
        {
            var resu = await _db.CyProduct.ToListAsync();
            return Ok(resu.Select(c => _mapper.Map<ProductDTO>(c)).ToList());
        }

        // GET: api/CyProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetCyProduct(int id)
        {

            var CyProduct = await _db.CyProduct.FindAsync(id);

            if (CyProduct == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductDTO>(CyProduct);
        }

        // PUT: api/CyProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCyProduct(int id, ProductDTO ProductDTO)
        {
            var CyProduct = _mapper.Map<CyProduct>(ProductDTO);
            if (id != CyProduct.ID)
            {
                return BadRequest();
            }

            _db.Entry(CyProduct).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CyProductExists(id))
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

        // POST: api/CyProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostCyProduct(ProductDTO ProductDTO)
        {
            var CyProduct = _mapper.Map<CyProduct>(ProductDTO);
            _db.CyProduct.Add(CyProduct);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetCyProduct", new { id = CyProduct.ID }, _mapper.Map<ProductDTO>(CyProduct));
        }

        // DELETE: api/CyProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCyProduct(int id)
        {
            var CyProduct = await _db.CyProduct.FindAsync(id);
            if (CyProduct == null)
            {
                return NotFound();
            }

            _db.CyProduct.Remove(CyProduct);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool CyProductExists(int id)
        {
            return _db.CyProduct.Any(e => e.ID == id);
        }
    }
}
