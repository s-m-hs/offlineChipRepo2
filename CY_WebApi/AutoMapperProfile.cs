using AutoMapper;
using CY_BM;
using CY_DM;

namespace CY_WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CyMenu, MenuDTO>().ReverseMap();
            CreateMap<CyMenuItem, MenuItemDTO>().ReverseMap();
            CreateMap<CyCategory, CategoryDTO>().ReverseMap();
            CreateMap<CySubject, SubjectDTO>().ReverseMap();
            CreateMap<CyUser, UserDTO>().ReverseMap();
            CreateMap<CySkin, SkinDTO>().ReverseMap();
            CreateMap<CyAddress, AddressDTO>().ReverseMap();
            CreateMap<CyAddress, AddressPutDTO>().ReverseMap();
            CreateMap<CyKeyData, KeyDataDTO>().ReverseMap();
            CreateMap<CyOrder, OrderDTO>().ReverseMap();
            CreateMap<CyOrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<OrderItemApproval,OrderItemApprovalDTO>().ReverseMap();
            CreateMap<CyOrder, OrderReturnDTO>().ReverseMap();
            CreateMap<CyOrderMessage, OrderMessageDTO>().ReverseMap();
            CreateMap<CyProduct, ProductDTO>().ReverseMap();
            CreateMap<CyManufacturer, ManufacturerDTO>().ReverseMap();
            CreateMap<CyProductSpec, ProductSpecDTO>();
            CreateMap<CyPCBOrder, PCBBDTO>().ReverseMap();
            CreateMap<PCBFile, PCBFileDTO>().ReverseMap();
            CreateMap<PCBOrderItemApproval, PCBOrderItemApprovalDTO>().ReverseMap();
            CreateMap<CyInquiryOrder, InquiryOrderDTO>().ReverseMap();
            CreateMap<CyOrderNotification, CyOrderNotificationDTO>().ReverseMap();

            CreateMap<ProductSpecDTO, CyProductSpec>().ForMember(c => c.IsVisible, c => c.MapFrom(src => true));


            CreateMap<CyProfile, ProfileDTO>().ReverseMap();
           // CreateMap<CyInspectionItem, InspectionItemDTO>().ReverseMap();
            CreateMap<CyProductCategory, ProductCategoryDTO>().ReverseMap();
           // CreateMap<CyPcbForm, pcbDTO>().ReverseMap();
           // CreateMap<CyGuarantee, GuaranteeDTO>().ReverseMap();
            CreateMap<OrderItemReturnDTO, CyOrderItem>().ReverseMap();
            CreateMap<AccessTable, AccessDTO>().ReverseMap();

            //CreateMap<CyOrder, InspectionOrderReturnDTO>().ForMember(dest => dest.InspectionForm, opt => opt.MapFrom(src => src.CyInspectionForm))
            //.ReverseMap();

            //CreateMap<CyInspectionForm, InspectionFormDTO>()
            //    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)).ReverseMap();


        }
    }
}
