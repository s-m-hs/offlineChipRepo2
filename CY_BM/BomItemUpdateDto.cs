using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class BomItemUpdateDto
    {
        public Guid? itemId {  get; set; }
        public int quantity { get; set; }
    }
}
