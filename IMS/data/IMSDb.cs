using IMS.Controllers;
using IMS.Models;
using Microsoft.EntityFrameworkCore;
namespace IMS.data
{
    public class IMSDb :DbContext
    {
        public IMSDb(DbContextOptions options):base(options) 
        { }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<SuppliersModel> Suppliers { get; set; }
        public DbSet<PurchasesModel> Purchases { get; set; }
        public DbSet<OrdersModel> Orders { get; set; }
    }
}
