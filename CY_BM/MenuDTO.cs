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
    public class MenuDTO
    {

        public Guid ID { get; set; }
        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        public required string NameCode { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class MenuItemDTO
    {
        public Guid ID { get; set; }

        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        [Display(Name = "اولیت نمایش")]
        public int OrderValue { get; set; }

        [Display(Name = "آدرس صفحه")]
        public string? PageUrl { get; set; }

        public bool? isProduct { get; set; }

        [Display(Name = "گروه")]
        public Guid? CyCategoryId { get; set; }
        public string? CategoryCode { get; set; }

        [Display(Name = "پوسته")]
        public Guid? CySkinId { get; set; }
        public string? SkinCode { get; set; }

        [Display(Name = "والد")]
        public Guid? rootId { get; set; }

        [Display(Name = "منو")]
        public Guid? CyMenuId { get; set; }

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

}
