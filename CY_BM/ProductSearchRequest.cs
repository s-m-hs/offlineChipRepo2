using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class ProductSearchRequest
    {
        public string Search { get; set; } = string.Empty;
        public string SortBy { get; set; } = "ID";  // پیش‌فرض مرتب‌سازی بر اساس ID
        public bool Descending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
