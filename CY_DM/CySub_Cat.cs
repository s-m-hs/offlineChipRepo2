using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CySub_Cat : BaseDM
    {
        public Guid? CySubjectID { get; set; }
        public virtual CySubject? CySubject { get; set; }
        public Guid? CyCategoryID { get; set; }
        public virtual CyCategory? CyCategory { get; set; }
    }
}