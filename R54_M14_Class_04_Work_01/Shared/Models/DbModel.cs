using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R54_M14_Class_04_Work_01.Shared.Models
{
    public enum Size { S = 1, M, L, XL }
    public class Product
    {
        public int ProductId { get; set; }
        [Required, StringLength(50)]
        public string ProductName { get; set; } = default!;
        [Required, EnumDataType(typeof(Size))]
        public Size? Size { get; set; } = default!;
        [Required, Column(TypeName = "money")]
        public decimal? Price { get; set; }
        [Required, StringLength(50)]
        public string? Picture { get; set; } = default!;

        public bool OnSale { get; set; }
        
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    }
    [ComplexType]
    public class Sale
    {
        public int SaleId { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [Required]
        public int? Quantity { get; set; }
        [Required, ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Sale> Sales { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            Random random = new();
            for (int i = 1; i <= 5; i++)
            {
                modelBuilder.Entity<Product>().HasData(
                    new Product { ProductId = i, ProductName = "Product " + i, Price = random.Next(1000, 2000), Size = (Size)random.Next(1, 5), Picture = i + ".jpg", OnSale = i % 2 == 0 }
                    );
            }
            for (int i = 1; i <= 8; i++)
            {
                modelBuilder.Entity<Sale>().HasData(
                    new Sale { SaleId = i, Date = DateTime.Now.AddDays(-1 * random.Next(400, 500)), ProductId = (i % 5 == 0 ? 5 : i % 5), Quantity = random.Next(100, 200) }
                    );
            }
        }
    }
}
