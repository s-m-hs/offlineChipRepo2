using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CY_DM;
using CY_WebApi.Models;
using AutoMapper;
using CY_BM;
using Microsoft.AspNetCore.Authorization;
using CY_WebApi.DataAccess;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyCategoriesController : ControllerBase
    {
        // private readonly CyContext _db;
        private readonly IMapper _mapper;
        private readonly Repository<CyCategory> _repo;
        private readonly CyContext _db;
        public CyCategoriesController(CyContext context, IMapper mapper, CyContext db)
        {
            _mapper = mapper;
            _repo = new Repository<CyCategory>(context);
            _db = db;
        }

        // GET: api/CyCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCyCategory()
        {
            var items = await _db.CyCategory.ToListAsync();
            return items.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }

        [HttpPost("getCategoriesPaged")]
        public async Task<ActionResult<IEnumerable<ManufacturerDTO>>> GetCyCategoryPaged(PageDTO PD)
        {
            var skipCount = PD.PageNumber * PD.PageSize;

            var allcount = await _db.CyCategory.Where(m => m.IsVisible).CountAsync();

            var resu = await _repo.TableNoTracking.OrderByDescending(c => c.ID).Skip(skipCount).Take(PD.PageSize).ToListAsync();
            var pagedres = new PageResultDTO<CategoryDTO>
            {

                ItemList = resu.Select(c => _mapper.Map<CategoryDTO>(c)).ToList(),
                AllCount = allcount,
            };
            return Ok(pagedres);
        }

        // GET: api/CyCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCyCategory(Guid id)
        {
            var cyCategory = await _repo.Find(id);// await _db.CyCategory.FindAsync(id);

            if (cyCategory == null || !cyCategory.IsVisible)
            {
                return NotFound();
            }

            return _mapper.Map<CategoryDTO>(cyCategory);
        }
        [HttpPost()]
        [Route("GetItemWChildAndRoot")]
        public async Task<ActionResult<CategoryDTO>> GetItemWChildAndRoot(GIDTO gdto)
        {
            var cyCategoryItem = await _db.CyCategory.Where(c => c.ID == gdto.ID && c.IsVisible).Include(c => c.rootCategory).Include(c => c.childItems).FirstOrDefaultAsync();

            if (cyCategoryItem == null)
            {
                return NotFound();
            }
            var MIWRAC = new CategoryWithRootAndChild()
            {
                Item = _mapper.Map<CategoryDTO>(cyCategoryItem),
                Root = cyCategoryItem.rootCategory is null ? null : _mapper.Map<CategoryDTO>(cyCategoryItem.rootCategory),
                Childs = cyCategoryItem.childItems is null ? null : cyCategoryItem.childItems.Where(c => c.IsVisible).Select(c => _mapper.Map<CategoryDTO>(c)).ToList()
            };


            return Ok(MIWRAC);
        }



        // PUT: api/CyCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCyCategory(Guid id, CategoryDTO Category)
        {
            var cyCategory = _mapper.Map<CyCategory>(Category);
            if (id != cyCategory.ID)
            {
                return BadRequest();
            }

            //  _db.Entry(cyCategory).State = EntityState.Modified;

            try
            {
                await _repo.Update(cyCategory);
                // await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CyCategoryExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/CyCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<CategoryDTO>> PostCyCategory(CategoryDTO Category)
        {
            var cyCategory = _mapper.Map<CyCategory>(Category);
            //_db.CyCategory.Add(cyCategory);
            //await _db.SaveChangesAsync();
            cyCategory = await _repo.Insert(cyCategory);
            return CreatedAtAction("GetCyCategory", new { id = cyCategory.ID }, _mapper.Map<CategoryDTO>(cyCategory));
        }

        // DELETE: api/CyCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCyCategory(Guid id)
        {
            //var cyCategory = await _db.CyCategory.FindAsync(id);
            //if (cyCategory == null)
            //{
            //    return NotFound();
            //}

            //_db.CyCategory.Remove(cyCategory);
            //await _db.SaveChangesAsync();
            await _repo.Delete(id);
            return NoContent();
        }

        //private bool CyCategoryExists(int id)
        //{
        //    return _db.CyCategory.Any(e => e.ID == id);
        //}
    }
}
