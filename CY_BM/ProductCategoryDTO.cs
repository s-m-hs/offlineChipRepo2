using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class ProductCategoryDTO
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }

        public required string Code { get; set; }
        public string? Url { get; set; }

        public int OrderValue { get; set; }
        public Guid? RootId { get; set; }

        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int ProductCount { get; set; }
        public ICollection<ProductCategoryDTO>? Childs { get; set; }

    }

        public class ProductCategoryWithRootAndChild
    {
        public ProductCategoryDTO? Item { get; set; }
        public ProductCategoryDTO? Root { get; set; }
        public List<ProductCategoryDTO>? Childs { get; set; }

    }

}
