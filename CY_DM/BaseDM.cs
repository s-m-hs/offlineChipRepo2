using System.ComponentModel;
namespace CY_DM
{
    public class BaseDM
    {
        public virtual Guid ID { get; set; }
        public virtual bool IsVisible { get; set; } = true;

       
        public virtual DateTime CreateDate { get; set; } = DateTime.Now;

        [DefaultValue("null")]
        public virtual DateTime? LastModified { get; set; }
    }
}
