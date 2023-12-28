
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R54_M14_Class_04_Work_01.Shared.Models;

namespace R54_M14_Class_04_Work_01.Shared.ViewModels
{
    public class ProductEditModel
    {
        public int ProductId { get; set; }
        [Required, StringLength(50)]
        public string ProductName { get; set; } = default!;
        [Required, EnumDataType(typeof(Size))]
        public Size? Size { get; set; } = default!;
        [Required, Column(TypeName = "money")]
        public decimal? Price { get; set; }
        [StringLength(50)]
        public string? Picture { get; set; } = default!;

        public bool OnSale { get; set; }
        [ValidateComplexType]
        public virtual List<Sale> Sales { get; set; } = new List<Sale>();
    }
}
