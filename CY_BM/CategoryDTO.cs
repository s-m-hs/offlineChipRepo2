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
    public class CategoryDTO
    {
        public Guid ID { get; set; }
        public required string Code { get; set; }
        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        [Display(Name = "الویت")]
        public int OrderValue { get; set; }

        public string? ImageUrl { get; set; }
        public int ProductCount { get; set; }
        public Guid? rootId { get; set; }

    }
    public class CategoryWithRootAndChild
    {
        public CategoryDTO Item { get; set; }
        public CategoryDTO? Root { get; set; }
        public List<CategoryDTO>? Childs { get; set; }

    }
   
}
