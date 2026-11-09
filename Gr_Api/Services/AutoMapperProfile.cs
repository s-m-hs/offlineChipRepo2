using AutoMapper;
using CY_BM;
using CY_DM;

namespace Gr_Api.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<GrManufacturer, GManufacturerDTO>().ReverseMap();
            CreateMap<GrProduct, GProductDTO>().ReverseMap();
            CreateMap<GrCategory, GCategoryDTO>().ReverseMap();
            CreateMap<GrKeyValue, GKeyValueDTO>().ReverseMap();
        }
    }
}
