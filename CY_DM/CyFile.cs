using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CY_DM
{
    public class CyFile : BaseDM
    {
        public Guid? FileIQ { get; set; }

        //  [StringLength(16)]
        public required string FolderName { get; set; }

        public Guid? SenderID { get; set; }
        [ForeignKey("SenderID")]
        public virtual CyUser? Sender { get; set; }

        public long FileSize { get; set; }
        //[StringLength(8)]
        public required string FileType { get; set; }
        // [StringLength(250)]
        public required string FileName { get; set; }
    }
}
