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
   
    public class SubjectDTO
    {
        public Guid ID { get; set; }
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

        public textEditorState? EditorState { get; set; }

        public string? bodyString { get; set; }

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
        public Guid? CyCategoryId { get; set; }
        public List<string>? CategoryName { get; set; }

        public Guid? CreateById { get; set; }

        public List<Guid>? CategoryIds { get; set; }
        //public virtual ICollection<CategorySubject> CategorySubjects { get; set; }
        //public virtual ICollection<GalleryItem> GalleryItems { get; set; }
        //public virtual ICollection<Post> Posts { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }
    }

   
}
