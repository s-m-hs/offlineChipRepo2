//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CY_DM;
//using CY_BM;
//using CY_WebApi.Models;
//using System.Runtime.InteropServices;
//using AutoMapper;
//using CY_WebApi.DataAccess;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CySkinsController : ControllerBase
//    {
//        private readonly IMapper _mapper;
//        private readonly Repository<CySkin> _repo;
//        public CySkinsController(CyContext context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _repo = new Repository<CySkin>(context);
//        }

//        // GET: api/CySkins
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<SkinDTO>>> GetCySkin()
//        {
//            var resu = await _repo.TableNoTracking.ToListAsync();// ; _db.CySkin.ToListAsync();
//            return Ok(resu.Select(c => _mapper.Map<SkinDTO>(c)).ToList());
//        }

//        // GET: api/CySkins/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<SkinDTO>> GetCySkin(int id)
//        {
//            var CySkin = await _repo.Find(id);
//            if (CySkin == null)
//            {
//                return NotFound();
//            }

//            return _mapper.Map<SkinDTO>(CySkin);
//        }

//        // PUT: api/CySkins/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutCySkin(int id, SkinDTO SkinDTO)
//        {
//            var CySkin = _mapper.Map<CySkin>(SkinDTO);
//            if (id != CySkin.ID)
//            {
//                return BadRequest("Not Valid ID");
//            }
//            try
//            {
//                await _repo.Update(CySkin);//  _db.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }

//            return NoContent();
//        }

//        // POST: api/CySkins
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<SkinDTO>> PostCySkin(SkinDTO SkinDTO)
//        {
//            var CySkin = _mapper.Map<CySkin>(SkinDTO);
//            CySkin = await _repo.Insert(CySkin);
//            return CreatedAtAction("GetCySkin", new { id = CySkin.ID }, _mapper.Map<SkinDTO>(CySkin));
//        }

//        // DELETE: api/CySkins/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCySkin(int id)
//        {
//            await _repo.Delete(id);
//            return NoContent();
//        }
//    }
//}
