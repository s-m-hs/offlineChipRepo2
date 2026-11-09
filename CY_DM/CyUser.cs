using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CY_BM;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
namespace CY_DM
{
    public class CyUser : BaseDM
    {
        public required string CyUsNm { get; set; }
        // [StringLength(400)]
        public required string CyHsPs { get; set; }

        public UserStatus Status { get; set; }
        [DefaultValue(6)]
        public int MaxWrongPass { get; set; }
        public int WrongPass { get; set; }

        public string? TTKK { get; set; }

        public Guid? CyProfileID { get; set; }
        public virtual CyProfile? CyProfile { get; set; }

        public Guid? AccessTableID { get; set; }
        public virtual AccessTable? AccessTable { get; set; }


        public Guid? CyGoroohID { get; set; }
        public CyGorooh? CyGorooh { get; set; }


        public Guid? CyVahedID { get; set; }
        public CyVahed? CyVahed { get; set; }


        public UserType userType { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public int RegisterCode { get; set; }
        public DateTime? RegisterCodeTimer { get; set; }


        [Display(Name = "والد")]
        public Guid? rootId { get; set; }
        [ForeignKey("rootId")]
        public virtual CyUser? rootCyUser { get; set; }

        [InverseProperty("rootCyUser")]
        public virtual ICollection<CyUser>? childUsers { get; }



    }
    public class CyUserOrder
    {
        public Guid OrderID { get; set; }
        public Guid UserID { get; set; }

    }





}
