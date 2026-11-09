//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CY_DM;
//using CY_WebApi.Models;
//using AutoMapper;
//using CY_BM;
//using CY_WebApi.DataAccess;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CyMenuItemsController : ControllerBase
//    {
//        private readonly Repository<CyMenuItem> _repo;
//        private readonly IMapper _mapper;

//        public CyMenuItemsController(CyContext context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _repo = new Repository<CyMenuItem>(context);
//        }

//        // GET: api/CyMenuItems
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<MenuItemDTO>>> GetCyMenuItem()
//        {
//            var items = await _repo.TableNoTracking.ToListAsync();
//            return items.Select(c => _mapper.Map<MenuItemDTO>(c)).ToList();

//        }

//        // GET: api/CyMenuItems/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<MenuItemDTO>> GetCyMenuItem(int id)
//        {
//            var cyMenuItem = await _repo.Find(id);

//            if (cyMenuItem == null)
//            {
//                return NotFound();
//            }

//            return _mapper.Map<MenuItemDTO>(cyMenuItem);
//        }

//        // GET: api/CyMenuItems/5
//        [HttpPost()]
//        [Route("GetItemWChildAndRoot")]
//        public async Task<ActionResult<MenuItemDTO>> GetItemWChildAndRoot(GIDTO gdto)
//        {
//            var cyMenuItem = await _repo.Table.Where(c => c.ID == gdto.ID &&c.IsVisible).Include(c => c.rootMenuItem).Include(c => c.childItems).FirstOrDefaultAsync();

//            if (cyMenuItem == null)
//            {
//                return NotFound();
//            }
//            var MIWRAC = new MenuItemWithRootAndChild()
//            {
//                Item = _mapper.Map<MenuItemDTO>(cyMenuItem),
//                Root = cyMenuItem.rootMenuItem is null ? null : _mapper.Map<MenuItemDTO>(cyMenuItem.rootMenuItem),
//                Childs = cyMenuItem.childItems is null ? null : cyMenuItem.childItems.Where(c=>c.IsVisible).Select(c => _mapper.Map<MenuItemDTO>(c)).ToList()
//            };


//            return Ok(MIWRAC);
//        }

//        // PUT: api/CyMenuItems/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutCyMenuItem(int id, MenuItemDTO MenuItemDTO)
//        {
//            var cyMenuItem = _mapper.Map<CyMenuItem>(MenuItemDTO);
//            if (id != cyMenuItem.ID)
//            {
//                return BadRequest();
//            }
//            //    _db.Entry(cyMenuItem).State = EntityState.Modified;

//            await _repo.Update(cyMenuItem);// _db.SaveChangesAsync();

//            return NoContent();
//        }

//        // POST: api/CyMenuItems
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<MenuItemDTO>> PostCyMenuItem(MenuItemDTO menuItemDTO)
//        {
//            var cyMenuItem = _mapper.Map<CyMenuItem>(menuItemDTO);
//            //_db.CyMenuItem.Add(cyMenuItem);
//            await _repo.Insert(cyMenuItem);// _db.SaveChangesAsync();

//            return CreatedAtAction("GetCyMenuItem", new { id = cyMenuItem.ID }, _mapper.Map<MenuItemDTO>(cyMenuItem));
//        }

//        // DELETE: api/CyMenuItems/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCyMenuItem(int id)
//        {
//            var cyMenuItem = await _repo.Find(id);// _db.CyMenuItem.FindAsync(id);
//            if (cyMenuItem == null)
//            {
//                return NotFound();
//            }
//            await _repo.Delete(id);// _db.SaveChangesAsync();

//            return NoContent();
//        }

//        //private bool CyMenuItemExists(int id)
//        //{
//        //    return _db.CyMenuItem.Any(e => e.ID == id);
//        //}
//    }
//}
