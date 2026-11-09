using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CY_BM
{
    public class ProdcutSearchDTO
    {
        public string? Name { get; set; }
        public string? ProductCategoryCode { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public string? CategoryCode { get; set; }
        public string? ManufacturerName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;

    }

    public class ProdcutSearchFliterDTO
    {
        public string? Name { get; set; }
        public Guid ProductCategoryId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public Dictionary<string, string>? FilterValues { get; set; }
    }
}