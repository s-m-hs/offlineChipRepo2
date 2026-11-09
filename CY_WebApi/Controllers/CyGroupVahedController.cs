using CY_BM;
using CY_DM;
using CY_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace CY_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CyGroupVahedController : ControllerBase
    {
        private readonly CyContext _db;
        public CyGroupVahedController(CyContext db)
        {
            _db = db;
            
        }


        [HttpPost("addGroup")]
        public async Task<ActionResult> addGroup([FromBody] GoroohDTO dto)
        {
            var group=await _db.CyGorooh.Where(x=>x.IsVisible && (x.Code == dto.Code || x.Text == dto.Text)).FirstOrDefaultAsync();

            if (group != null) return BadRequest(new {msg=" نام یا کد تکراری است"});

            var vahed=await _db.CyVahed.Where(x=>x.IsVisible && x.ID== dto.VahedId).FirstOrDefaultAsync();
            if (vahed == null) return BadRequest("vahed notFound");

            CyGorooh newGrooh=new CyGorooh() {
            Code = dto.Code.Trim(),
            Text = dto.Text.Trim(),
            VahedId= (Guid) dto.VahedId,
            };

            await _db.CyGorooh.AddAsync(newGrooh);
            await _db.SaveChangesAsync();

            return Ok(dto);
        }

        [HttpGet("getGroup")]
        public async Task<ActionResult<CyGorooh>> getGroup(Guid? id)
        {
            var query=_db.CyGorooh.Where(x=>x.IsVisible).ToList();

            if (id == null)
            {
                return Ok(query);
            }
      
              var  grooh=query.Where(x=>x.ID == id).FirstOrDefault();


            return Ok(grooh);
        }

        [HttpGet("getGroupByVahed")]
        public async Task<ActionResult> getGroupByVahed(Guid id)
        {
            var groops=await _db.CyGorooh.Where(x=>x.IsVisible && x.VahedId == id).ToListAsync();

            return Ok(groops);

        }

        [HttpPut("editGroup")]
        public async Task<ActionResult> editgroup([FromBody] CyGorooh gorooh )
        {

            var group=_db.CyGorooh.Where(x=>x.IsVisible && x.ID == gorooh.ID).FirstOrDefault();
            if (group == null) return BadRequest(new {msg="group notFound"});

            var vahed =await _db.CyVahed.Where(x => x.IsVisible && x.ID == gorooh.VahedId).FirstOrDefaultAsync();
            if (vahed == null) return BadRequest("vahed notFound");

            group.Text = gorooh.Text.Trim();
            group.Code = gorooh.Code.Trim();
            group.VahedId= gorooh.VahedId;

            await _db.SaveChangesAsync();

            return Ok(group);   


        }


        [HttpDelete("deletGroup")]
        public async Task<ActionResult> deletGroup(Guid? id)
        {
           var group=_db.CyGorooh.Where(x=>x.IsVisible && x.ID == id).FirstOrDefault();
            
            if (group == null) return BadRequest(new { msg = "group notFound" });

            group.IsVisible = false;

            await _db.SaveChangesAsync();
            return Ok(new {msg="گروه با موفقیت حذف شد"});                                                   
        }




        [HttpPost("addVahed")]
        public async Task<ActionResult> addVahed([FromBody] VahedDTO dto)
        {
            var cuVahed = await _db.CyVahed.Where(x => x.IsVisible && (x.Code == dto.Code || x.Text == dto.Text)).FirstOrDefaultAsync();

            if (cuVahed != null) return BadRequest(new { msg = " نام یا کد تکراری است" });

            CyVahed newVahed = new CyVahed()
            {
                Code = dto.Code.Trim(),
                Text = dto.Text.Trim(),
            };

            await _db.CyVahed.AddAsync(newVahed);
            await _db.SaveChangesAsync();

            return Ok(dto);
        }


        [HttpGet("getVahed")]
        public async Task<ActionResult<CyVahed>> getVahed(Guid? id)
        {
            var query = _db.CyVahed.Where(x => x.IsVisible).ToList();

            if (id == null)
            {
                return Ok(query);
            }

            var Vahed = query.Where(x => x.ID == id).FirstOrDefault();


            return Ok(Vahed);
        }


        [HttpPut("editVahed")]
        public async Task<ActionResult> editVahed([FromBody] CyVahed Vahed)
        {

            var cuVahed = _db.CyVahed.Where(x => x.IsVisible && x.ID == Vahed.ID).FirstOrDefault();
            if (cuVahed == null) return BadRequest(new { msg = "Vahed notFound" });

            cuVahed.Text = Vahed.Text.Trim();
            cuVahed.Code = Vahed.Code.Trim();

            await _db.SaveChangesAsync();

            return Ok(cuVahed);


        }


        [HttpDelete("deletVahed")]
        public async Task<ActionResult> deletVahed(Guid id)
        {
            var Vahed = _db.CyVahed.Where(x => x.IsVisible && x.ID == id).FirstOrDefault();

            if (Vahed == null) return BadRequest(new { msg = "Vahed notFound" });

            Vahed.IsVisible = false;

            await _db.SaveChangesAsync();
            return Ok(new { msg = " با موفقیت حذف شد" });
        }


    }
}
