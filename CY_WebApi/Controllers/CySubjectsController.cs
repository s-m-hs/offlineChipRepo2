//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CY_WebApi.Models;
//using CY_DM;
//using CY_BM;
//using AutoMapper;
//using CY_WebApi.DataAccess;
//using System.Text;
//using System.Linq.Dynamic.Core;
//using System.Security.Claims;
//using Microsoft.Identity.Client;
//using CY_WebApi.Services;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CySubjectsController : ControllerBase
//    {
//        private readonly Repository<CySubject> _repo;
//        private readonly IMapper _mapper;
//        private readonly CyContext _db;


//        public CySubjectsController(CyContext context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _repo = new Repository<CySubject>(context);
//            _db = context;
//        }

//        // GET: api/CySubjects
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetCySubject()
//        {
//            var resu = await _repo.TableNoTracking.ToListAsync();
//            return Ok(resu.Select(c => _mapper.Map<SubjectDTO>(c)).ToList());
//        }

//        // GET: api/CySubjects/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<SubjectDTO>> GetCySubject(int id)
//        {
//            var cySubject = await _repo.Find(id);

//            if (cySubject == null || !cySubject.IsVisible)
//            {
//                return NotFound();
//            }

//            return _mapper.Map<SubjectDTO>(cySubject);
//        }

//        // PUT: api/CySubjects/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        [TypeAuthorize([UserType.SalesManager])]
//        public async Task<IActionResult> PutCySubject(int id, SubjectDTO Subject)
//        {
//            var cySubject = _mapper.Map<CySubject>(Subject);
//            if (id != cySubject.ID)
//            {
//                return BadRequest();
//            }

//            await _repo.Update(cySubject);
//            var sub_cats = await _db.CySub_Cats.Where(sc => sc.IsVisible && sc.CySubjectID == Subject.ID).ToListAsync();
//            foreach (var c in sub_cats)
//            {
//                c.IsVisible = false;
//            }
//            if (Subject.CategoryIds != null && Subject.CategoryIds.Any())
//            {
//                foreach (var c in Subject.CategoryIds)
//                {
//                    _db.CySub_Cats.Add(new CySub_Cat()
//                    {
//                        CySubjectID = Subject.ID,
//                        CyCategoryID = c,
//                        IsVisible = true,
//                        CreateDate = DateTime.Now,
//                    });
//                }
//            }
//            await _db.SaveChangesAsync();
//            return NoContent();
//        }

//        [Route("GetSubjectByCat")]
//        [HttpPost]
//        public async Task<ActionResult<PageResultDTO<SubjectDTO>>> GetSubjectByCat(PageDTO PD)
//        {

//            var nowDate = DateTime.Now;
//            if(PD.PageSize == 0)
//            {
//                var cat = await _db.CyCategory.Where(c => c.Code == PD.Cat && c.IsVisible).FirstOrDefaultAsync();
//                if(cat != null)
//                    PD.PageSize = cat.ProductCount;
//            }
//            var skipCount = PD.PageNumber * PD.PageSize;
//          // _repo.Table.Where(x=>EF.Functions.FreeText(x.))
                
//            var allcount = await _repo.Table.Where(c => c.CyCategoryId.HasValue && c.CyCategory.Code == PD.Cat
//             && c.IsVisible && c.IsAuthenticate
//             ).CountAsync();

//            var SubjList = await _repo.Table.Where(c => c.CyCategoryId.HasValue && c.CyCategory.Code == PD.Cat
//            && c.IsVisible && c.IsAuthenticate
//            )
//                .Include(c => c.CyCategory)
//                .OrderBy(c => c.OrderValue).Skip(skipCount).Take(PD.PageSize).ToListAsync();
//            if (SubjList.Count > 0)
//            {
//                var menuWitems = new PageResultDTO<SubjectDTO>
//                {
//                    PageTitle = SubjList[0]?.CyCategory?.Text,
//                    ItemList = SubjList.Select(c => _mapper.Map<SubjectDTO>(c)).ToList(),
//                    AllCount = allcount,
//                };
//                return Ok(menuWitems);
//            }
//            return NotFound();
//        }

//        [HttpPost("GetSearchedSubject")]
//        public async Task<ActionResult<PageResultDTO<SubjectDTO>>> GetSearchedSubject(SearchSubjectDTO PD)
//        {

//            var skipCount = PD.PageNumber * PD.PageSize;

//            /////Start Searching/////
//            var stringProperties = typeof(CySubject).GetProperties()
//                                    .Where(p => p.PropertyType == typeof(string))
//                                    .ToList();

//            var query = _repo.Table.AsQueryable();

//            if (!string.IsNullOrWhiteSpace(PD.SearchStr))
//            {
//                var searchQuery = new StringBuilder();
//                var parameters = new List<object>();

//                for (int i = 0; i < stringProperties.Count; i++)
//                {
//                    if (i > 0)
//                    {
//                        searchQuery.Append(" OR ");
//                    }

//                    searchQuery.Append($"{stringProperties[i].Name}.Contains(@{i})");
//                    parameters.Add(PD.SearchStr);
//                }
//                query = query.Where(searchQuery.ToString(), parameters.ToArray());
//            }

//            var allcount = await query.Where(s => s.IsVisible)
//                                 //.OrderBy(c => c.CreateDate)
//                                 .CountAsync();

//            var res = await query.Where(s=> s.IsVisible)
//                                 .OrderBy(c => c.CreateDate)
//                                 .Skip(skipCount).Take(PD.PageSize)
//                                 .ToListAsync();
//            /////End Searching/////

//            //var allcount = res.Count;

//            var pagedRes = new PageResultDTO<SubjectDTO>
//            {
//                AllCount = allcount,
//                ItemList = res.Select(c => _mapper.Map<SubjectDTO>(c)).ToList(),
//            };

//            return Ok(pagedRes);

//        }


//        [HttpPost("SearchSubjectForAdmin")]
//        [TypeAuthorize([UserType.SalesManager])]
//        public async Task<IActionResult> SearchSubjectForAdmin(SubjectSearchDTO input)
//        {

//            var skipCount = input.PageNumber * input.PageSize;

//            var Query = _repo.Table.Where(s => s.IsVisible);
//            if (input.Title != null)
//            {
//                Query = Query.Where(s => s.Title.Contains(input.Title));
//            }
//            if (input.CategoryCode != null)
//            {
//                Query = Query.Where(s => s.CyCategory.Code == input.CategoryCode);
//            }
//            var finalResult = await Query.Skip(skipCount).Take(input.PageSize).ToListAsync();
//            if(finalResult != null)
//            {
//                var pagedres = new PageResultDTO<SubjectDTO>
//                {
//                    ItemList = _mapper.Map<List<SubjectDTO>>(finalResult),
//                    AllCount = Query.Count(),
//                };
//                return Ok(pagedres);            
//            }
//            else
//                return NoContent();
//        }

//        [HttpPost("getSubjectsByCategoryId")]
//        public async Task<ActionResult<PageResultDTO<SubjectDTO>>> getSubjectByCategoryId(PageSubCatDTO inputs)
//        {
//            var skipCount = inputs.PageNumber * inputs.PageSize;

//            var subjectIds = await _db.CySub_Cats.Where(c=> c.IsVisible && c.CyCategoryID == inputs.CatId).Select(c=> c.CySubjectID).ToListAsync();

//            var subjects = await _db.CySubject.Where(s => s.IsVisible && subjectIds.Contains(s.ID)).Skip(skipCount).Take(inputs.PageSize)
//                .
//                Select(s => new SubjectDTO()
//                {
//                    Title = s.Title,
//                    BigImg = s.BigImg,
//                    Body = s.Body,
//                    CyCategoryId = inputs.CatId,
//                    CySkinId = s.CySkinId,
//                    ID = s.ID,
//                    URL_Title = s.URL_Title,
//                    Tag = s.Tag,
//                    SmallImg = s.SmallImg,
//                    PreTitle = s.PreTitle,
//                    OrderValue = s.OrderValue,
//                    IsAuthenticate = s.IsAuthenticate,
//                    Extra = s.Extra,
//                    Describtion = s.Describtion,
//                    DateShow = s.DateShow,
//                    DateExp = s.DateExp,
//                    CreateById = s.CreateById,
//                    CategoryName = _db.CySub_Cats.Where(c => c.IsVisible && c.CySubjectID == s.ID && c.CyCategoryID != 685).Select(c => c.CyCategory.Text).ToList() //685 is for maghalat.
//                })
//                .ToListAsync();
//            var pagedResult = new PageResultDTO<SubjectDTO>
//            {
//                AllCount = subjectIds.Count(),
//                ItemList = subjects
//            };
//            return Ok(pagedResult);
//        }

//        [HttpGet("getCategoryIdsForSubject")]
//        [TypeAuthorize([UserType.SalesManager])]
//        public async Task<ActionResult<CySubject>> getCategoriesForSubject(int subjectId)
//        {
//            var categoryIds = await _db.CySub_Cats.Where(c => c.IsVisible && c.CySubjectID == subjectId).Select(c=> c.CyCategoryID).ToListAsync();
//            return Ok(categoryIds);
//        }
//        // POST: api/CySubjects
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<CySubject>> PostCySubject(SubjectDTO Subject)
//        {
//            var cySubject = _mapper.Map<CySubject>(Subject);

//            await _repo.Insert(cySubject);
//            if (Subject.CategoryIds != null && Subject.CategoryIds.Any())
//            {
//                var sub_cats = await _db.CySub_Cats.Where(sc => sc.IsVisible && sc.CyCategoryID != null && Subject.CategoryIds.Contains(sc.CyCategoryID.Value) && sc.CySubjectID == Subject.ID).ToListAsync();
//                foreach (var c in sub_cats)
//                {
//                    c.IsVisible = false;
//                }
//                foreach (var c in Subject.CategoryIds)
//                {
//                    _db.CySub_Cats.Add(new CySub_Cat()
//                    {
//                        CySubjectID = Subject.ID,
//                        CyCategoryID = c,
//                        IsVisible = true,
//                        CreateDate = DateTime.Now,
//                    });
//                }
//                await _db.SaveChangesAsync();
//            }
//            return CreatedAtAction("GetCySubject", new { id = cySubject.ID }, cySubject);
//        }

//        // DELETE: api/CySubjects/5
//        [HttpDelete("{id}")]
//        [TypeAuthorize([UserType.SalesManager])]
//        public async Task<IActionResult> DeleteCySubject(int id)
//        {
//            await _repo.Delete(id);

//            return NoContent();
//        }
//    }
//}