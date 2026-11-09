using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class SubjectSearchDTO
    {
        public string? Title { get; set; }
        public string? CategoryCode { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
