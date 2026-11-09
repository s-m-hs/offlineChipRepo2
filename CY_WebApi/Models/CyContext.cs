using Microsoft.EntityFrameworkCore;
using CY_DM;
using CY_WebApi.Models;
namespace CY_WebApi.Models
{
    public class CyContext:DbContext
    {
        public CyContext(DbContextOptions<CyContext> options)
        : base(options)
        {
                    
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         //   optionsBuilder .UseSqlServer("OurConnectionString");
        }

        public DbSet<CyMenu> CyMenus { get; set; } = default!;
        public DbSet<CY_DM.CyCategory> CyCategory { get; set; } = default!;
        public DbSet<CY_DM.CyFile> CyFile { get; set; } = default!;
        public DbSet<CY_DM.CyKeyData> CyKeyData { get; set; } = default!;
        public DbSet<CY_DM.CyListMail> CyListMail { get; set; } = default!;
        public DbSet<CY_DM.CyMenuItem> CyMenuItem { get; set; } = default!;
        public DbSet<CY_DM.CyOrder> CyOrder { get; set; } = default!;
        public DbSet<CY_DM.CyOrderItem> CyOrderItem { get; set; } = default!;
        public DbSet<CY_DM.CyAddress> CyAddress { get; set; } = default!;
        public DbSet<CY_DM.CyProduct> CyProduct { get; set; } = default!;
        public DbSet<CY_DM.CyProfile> CyProfile { get; set; } = default!;
     //   public DbSet<CY_DM.CyTicket> CyTicket { get; set; } = default!;
        public DbSet<CY_DM.CyUser> CyUser { get; set; } = default!;
        public DbSet<CY_DM.CySubject> CySubject { get; set; } = default!;
        public DbSet<CY_DM.CySkin> CySkin    { get; set; } = default!;
        public DbSet<CY_DM.CyManufacturer> CyManufacturer { get; set; } = default!;
        public DbSet<CY_DM.CyOrderMessage> CyOrderMessage { get; set; } = default!;
        public DbSet<CY_DM.CyProductSpec> CyProductSpec { get; set; } = default!;
      //  public DbSet<CY_DM.CyInspectionItem> CyInspectionItem { get; set; } = default!;
      //  public DbSet<CY_DM.CyInspectionForm> CyInspectionForm { get; set; } = default !;
      //  public DbSet<CY_DM.CyPcbForm> CyPcbForm { get; set; } = default!;
        public DbSet<CY_DM.CyProductCategory> CyProductCategory { get; set; } = default!;
      //  public DbSet<CY_DM.CyGuarantee> CyGuarantee { get; set; } = default!;
        public DbSet<CY_DM.CySub_Cat> CySub_Cats { get; set; } = default!;
        public DbSet<CY_DM.AccessTable> AccessTable { get; set; } = default!;
        public DbSet<CY_DM.CyGorooh> CyGorooh { get; set; } = default!;
        public DbSet<CY_DM.CyVahed> CyVahed { get; set; } = default!;
        public DbSet<CY_DM.OrderItemApproval> OrderItemApproval { get; set; } = default!;
        public DbSet<CY_DM.PCBOrderItemApproval> PCBOrderItemApproval { get; set; } = default!;
        public DbSet<CY_DM.PCBFile> PCBFile { get; set; } = default!;
        public DbSet<CY_DM.CyPCBOrder> CyPCBOrder { get; set; } = default!;
        public DbSet<CY_DM.CyInquiryOrder> CyInquiryOrder { get; set; } = default!;

        public DbSet<CY_DM.CyUserOrder> CyUserOrders { get; set; } = default!;
        public DbSet<CY_DM.CyOrderNotification> CyOrderNotification { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CyUserOrder>()
              .HasKey(od => new { od.UserID, od.OrderID });
    
        }
    }
}