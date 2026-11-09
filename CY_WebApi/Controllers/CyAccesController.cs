using AutoMapper;
using CY_BM;
using CY_DM;
using CY_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyAccesController:ControllerBase
    {
        private readonly CyContext _db;
        private readonly IMapper _mapper;

        public CyAccesController(CyContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            
        }

        [HttpPost("addAccess")]
        public async Task<ActionResult> addAccess([FromBody] AccessDTO dto)
        {
            var acceseSameName =await _db.AccessTable.Where(x => x.IsVisible && x.Text == dto.Text).FirstOrDefaultAsync();
            if (acceseSameName != null) return BadRequest("this name is avalable");

            var newAccess=_mapper.Map<AccessTable>(dto);

            await _db.AccessTable.AddAsync(newAccess);

            await _db.SaveChangesAsync();

            return Ok(dto);
        }



        [HttpGet("getAccess")]
        public async Task<ActionResult<AccessTable>> getAccess(Guid? id)
        {

            var query=_db.AccessTable.Where(x=>x.IsVisible).ToList();

            if (id == null)
            {
                return Ok(query);
            }
            else
            {
                query.Where(x=>x.ID == id).FirstOrDefault();
            }

            return Ok(query);
        }


        [HttpPut("editAccess")]
        public async Task<ActionResult> editAccess([FromBody] AccessDTO dto,Guid? id)
        {
            var cuAccess = await _db.AccessTable
                .Where(x => x.IsVisible && x.ID == id)
                .FirstOrDefaultAsync();

            if (cuAccess == null)
                return BadRequest("access not found");

            bool hasChange = false;

            var dtoProps = dto.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(bool));

            foreach (var item in dtoProps)
            {
                var entityProp = cuAccess.GetType().GetProperty(item.Name);
                if (entityProp == null || entityProp.PropertyType != typeof(bool))
                    continue;

                bool dtoValue = (bool)item.GetValue(dto)!;
                bool entityValue = (bool)entityProp.GetValue(cuAccess)!;

                if (dtoValue != entityValue)
                {
                    entityProp.SetValue(cuAccess, dtoValue);
                    hasChange = true;
                }
            }

            if (cuAccess.Text.Trim() != dto.Text.Trim()) 
            { 
                cuAccess.Text = dto.Text.Trim();
                hasChange = true;
            }

            if (!hasChange)
                return Ok(new { msg = "no changes" });

            await _db.SaveChangesAsync();

            return Ok(new { msg = "updated" });
        }

        [HttpDelete("deleteAccess")]
        public async Task<ActionResult> deleteAccess(Guid id)
        {
            var access=await _db.AccessTable.Where(x=>x.IsVisible && x.ID == id).FirstOrDefaultAsync();
            if (access == null) return BadRequest("access not found");
            access.IsVisible = false ;

            _db.SaveChanges();

            return Ok();


        }
    }
}
