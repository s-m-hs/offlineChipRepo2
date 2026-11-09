using CY_BM;
using CY_DM;
using CY_WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Linq.Dynamic.Core;

namespace CY_WebApi.Services
{
    public class OrderCodeService
    {
        private readonly CyContext _db;
        public OrderCodeService(CyContext db)
        {
            _db = db;
        }

        //public async Task<int> GeneratCode(Ordermode mode)
        //{
        //    var lastItem = await _db.CyOrder.AsNoTracking().Where(x => x.OrderMode == mode).OrderBy(x => x.OrderCode).LastOrDefaultAsync();

        //    if (mode == Ordermode.Inspection)
        //    {
        //        if (lastItem == null || lastItem.OrderCode == 0)
        //            return 8000;
        //        else
        //            return lastItem.OrderCode + 1;
        //    }
        //    else if (mode == Ordermode.Pcb)
        //    {
        //        if (lastItem == null || lastItem.OrderCode == 0)
        //            return 10000;
        //        else
        //            return lastItem.OrderCode + 1;
        //    }
        //    else if (mode == Ordermode.Bom || mode == Ordermode.Normal )
        //    {
        //        if (lastItem == null || lastItem.OrderCode == 0)
        //            return 20000;
        //        else
        //            return lastItem.OrderCode + 1;
        //    }
        //    return -1;
        //}
    }
}
