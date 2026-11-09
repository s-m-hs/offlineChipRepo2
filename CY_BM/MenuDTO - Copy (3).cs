using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class UserDTO
    {
        public int ID { get; set; }
        public required string CyUsNm { get; set; }
        // [StringLength(400)]
        public required string CyHsPs { get; set; }

        public UserStatus Status { get; set; }

        public UserType userType { get; set; }
    }
    public class MenuDTO
    {

        public int ID { get; set; }
        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        public required string NameCode { get; set; }

        public string? ImageUrl { get; set; }
    }
    public class MenuItemDTO
    {
        public int ID { get; set; }

        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        [Display(Name = "اولیت نمایش")]
        public int OrderValue { get; set; }

        [Display(Name = "آدرس صفحه")]
        public string? PageUrl { get; set; }

        public bool? isProduct { get; set; }

        [Display(Name = "گروه")]
        public int? CyCategoryId { get; set; }
        public string? CategoryCode { get; set; }

        [Display(Name = "پوسته")]
        public int? CySkinId { get; set; }
        public string? SkinCode { get; set; }

        [Display(Name = "والد")]
        public int? rootId { get; set; }

        [Display(Name = "منو")]
        public int? CyMenuId { get; set; }

        public string? Meta { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class MenuItemWithRootAndChild
    {
        public MenuItemDTO Item { get; set; }
        public MenuItemDTO? Root { get; set; }
        public List<MenuItemDTO>? Childs { get; set; }

    }
    public class MenuWithItems
    {
        public MenuDTO? Menu { get; set; }
        public List<MenuItemDTO>? Items { get; set; }
    }
    public class GIDTO
    {
        public Guid? GID { get; set; }
        public int? ID { get; set; }
        public string? Str { get; set; }
    }
    public class CategoryDTO
    {
        public int ID { get; set; }
        public required string Code { get; set; }
        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        [Display(Name = "الویت")]
        public int OrderValue { get; set; }

        public string? ImageUrl { get; set; }
        public int ProductCount { get; set; }
        public int? rootId { get; set; }

    }
    public class CategoryWithRootAndChild
    {
        public CategoryDTO Item { get; set; }
        public CategoryDTO? Root { get; set; }
        public List<CategoryDTO>? Childs { get; set; }

    }
    public class SubjectDTO
    {
        public int ID { get; set; }
        [Display(Name = "پیش عنوان")]
        public string? PreTitle { get; set; }

        [Required(ErrorMessage = "لطفا عنوان را وارد کنید")]

        [Display(Name = "عنوان")]
        public required string Title { get; set; }

        [Display(Name = "متن آدرس")]
        public string? URL_Title { get; set; }

        [Display(Name = "خلاصه مطلب")]
        [DataType(DataType.MultilineText)]
        public string? Describtion { get; set; }



        [Display(Name = "متن اصلی")]
        [DataType(DataType.MultilineText)]
        public string? Body { get; set; }


        [Display(Name = "کلمات کلیدی")]
        public string? Tag { get; set; }


        [Display(Name = "فیلد اضافه")]
        public string? Extra { get; set; }

        [Display(Name = "تاریخ انتشار")]
        public DateTime? DateShow { get; set; }

        [Display(Name = "تاریخ انقضا")]
        public DateTime? DateExp { get; set; }

        [Display(Name = "تایید شده")]
        public bool IsAuthenticate { get; set; }


        [Display(Name = "آدرس تصویر بزرگ")]
        public string? BigImg { get; set; }


        [Display(Name = "آدرس عکس کوچک")]
        public string? SmallImg { get; set; }

        //[Display(Name = "تعداد نمایش")]
        //public int CountVisit { get; set; }

        //[Display(Name = "تعداد پست")]
        //public int CountPost { get; set; }

        [Display(Name = "اولیت نمایش")]
        public int OrderValue { get; set; }


        public int? CySkinId { get; set; }

        [Display(Name = "گروه اصلی ")]
        public int? CyCategoryId { get; set; }

        public int? CreateById { get; set; }


        //public virtual ICollection<CategorySubject> CategorySubjects { get; set; }
        //public virtual ICollection<GalleryItem> GalleryItems { get; set; }
        //public virtual ICollection<Post> Posts { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }
    }

    public class LoginModel
    {
        public string Un { get; set; }
        public string Pw { get; set; } // For demonstration only, avoid storing plain text passwords
    }

    public class PageDTO
    {
        public string Cat { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
    }
    public class PageResultDTO<T> where T : class
    {
        public List<T> ItemList { get; set; }
        public int AllCount { get; set; }
    }

    public class KeyDataDTO
    {
        public int ID { get; set; }
        [Display]
        public required string Key { get; set; }

        public string? Tag { get; set; }

        [Display(Name = "مقدار")]
        public required string Value { get; set; }

    }
    public class SkinDTO
    {
        public int ID { get; set; }

        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        public required string Code { get; set; }

    }
    public class ProductDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public int? Price { get; set; }
        public int? NoOffPrice { get; set; }
        public string? PartNo { get; set; }
        public string? MfrNo { get; set; }
        public string? DatasheetUrl { get; set; }
        public int Supply { get; set; }

        // public string? DataSheet { get; set; }
        // public string? RoHS { get; set; }

        public string? MainImage { get; set; }
        public string? SmallImage { get; set; }
        public string? Images { get; set; }
        public int? CyManufacturerId { get; set; }
        [Display(Name = "گروه اصلی ")]
        public int? CyCategoryId { get; set; }

        public List<ProductSpecDTO>? Spec {  get; set; }
        // Navigation properties
        // public virtual ICollection<CyProductPriceAndWarranty>? PricesAndWarranties { get; set; }
        // public virtual ICollection<CyProductSpec>? Specifications { get; set; }

    }
    public class ProductSpecDTO 
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public int CyProductId { get; set; }
        // Navigation property
    }

    public class ManufacturerDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation property
    }
    public class OutFileModel
    {
        public int? ID { get; set; }
        public string? Adress { get; set; }
    }

    public class ProfileDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        // [StringLen   gth(40)]
        public string? Family { get; set; }
        // [StringLength(40)]
        public string? Email { get; set; }
        //   [StringLength(40)]
        public string? Website { get; set; }
        //  [StringLength(40)]
        public string? Mobile { get; set; }

        public string? Description { get; set; }

        public string? UserImageUrl { get; set; }

    }
}
