//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CY_DM;
//using CY_WebApi.Models;
//using CY_BM;
//using AutoMapper;
//using CY_WebApi.DataAccess;
//using Microsoft.VisualBasic;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CyMenusController : ControllerBase
//    {
//        private readonly Repository<CyMenu> _repo;
//        private readonly Repository<CyMenuItem> _repoMenuItem;
//        private readonly IMapper _mapper;
//        public CyMenusController(CyContext context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _repo = new Repository<CyMenu>(context);
//            _repoMenuItem = new Repository<CyMenuItem>(context);
//        }

//        [Route("GetMenuWithItems")]
//        [HttpPost]
//        public async Task<ActionResult<MenuWithItems>> GetMenuWithItems(GIDTO gdto)
//        {
//            var CMenu = await _repo.Table.Where(c => c.ID == gdto.ID && c.IsVisible).Include(c => c.CyMenuItems).FirstOrDefaultAsync();
//            if (CMenu != null)
//            {
//                var menuWitems = new MenuWithItems()
//                {
//                    Menu = _mapper.Map<MenuDTO>(CMenu),
//                    Items = CMenu.CyMenuItems != null ? CMenu.CyMenuItems.Where(c => c.IsVisible).Select(c => _mapper.Map<MenuItemDTO>(c)).ToList() : null,
//                };
//                return Ok(menuWitems);
//            }
//            return NotFound();
//        }
//        [Route("GetMenuByCodeWithItems")]
//        [HttpPost]
//        public async Task<ActionResult<MenuWithItems>> GetMenuByCodeWithItems(GIDTO gdto)
//        {
//            var CMenu = await _repo.Table.Where(c => c.NameCode == gdto.Str && c.IsVisible).FirstOrDefaultAsync();
//            var CMenuItems = await _repoMenuItem.Table.Where(c => c.CyMenuId != null &&
//                                                                c.CyMenu.NameCode == gdto.Str &&
//                                                                c.CyMenu.IsVisible && c.IsVisible)
//                                                                .Include(c=>c.CySkin).Include(c=>c.CyCategory).Select(c =>
//                new MenuItemDTO()
//                {
//                    Text = c.Text,
//                    CategoryCode = c.CyCategory != null ? c.CyCategory.Code : "",
//                    CyCategoryId = c.CyCategoryId,
//                    CyMenuId = c.CyMenuId,
//                    CySkinId = c.CySkinId,
//                    ID = c.ID,
//                    ImageUrl = c.ImageUrl,
//                    Meta = c.Meta,
//                    OrderValue = c.OrderValue,
//                    PageUrl = c.PageUrl,
//                    rootId = c.rootId,
//                    SkinCode = c.CySkin != null ? c.CySkin.Code : "",
//                }
//                ).ToListAsync();
//            if (CMenu != null)
//            {
//                var menuWitems = new MenuWithItems()
//                {
//                    Menu = _mapper.Map<MenuDTO>(CMenu),
//                    Items = CMenuItems
//                };
//                return Ok(menuWitems);
//            }
//            return NotFound();
//        }
//        // GET: api/CyMenus
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<MenuDTO>>> GetCyMenus()
//        {
//            var resu = await _repo.TableNoTracking.ToListAsync();
//            return Ok(resu.Select(c => _mapper.Map<MenuDTO>(c)).ToList());

//        }

//        // GET: api/CyMenus/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<MenuDTO>> GetCyMenu(int id)
//        {
//            var cyMenu = await _repo.Find(id);

//            if (cyMenu == null)
//            {
//                return NotFound();
//            }

//            return _mapper.Map<MenuDTO>(cyMenu);
//        }

//        // PUT: api/CyMenus/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutCyMenu(int id, MenuDTO menuDTO)
//        {
//            var cyMenu = _mapper.Map<CyMenu>(menuDTO);
//            if (id != cyMenu.ID)
//            {
//                return BadRequest();
//            }

//            await _repo.Update(cyMenu);

//            return NoContent();
//        }

//        // POST: api/CyMenus
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<MenuDTO>> PostCyMenu(MenuDTO MenuItemDto)
//        {
//            var cyMenu = _mapper.Map<CyMenu>(MenuItemDto);
//            cyMenu.IsVisible = true;
//            cyMenu = await _repo.Insert(cyMenu);

//            return CreatedAtAction("GetCyMenu", new { id = cyMenu.ID }, _mapper.Map<MenuDTO>(cyMenu));
//        }

//        // DELETE: api/CyMenus/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCyMenu(int id)
//        {
//            var cyMenu = await _repo.Find(id);
//            if (cyMenu == null)
//            {
//                return NotFound();
//            }

//            await _repo.Delete(id);

//            return NoContent();
//        }

//        //private bool CyMenuExists(int id)
//        //{
//        //    return _db.CyMenus.Any(e => e.ID == id);
//        //}
//    }
//}
