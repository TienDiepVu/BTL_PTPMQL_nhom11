using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models
{
    [Table("NhaCungCap")]
    public class NhaCungCap 
    {
        [Key]
        public required string MaNhaCungCap { get; set; }
        public required string TenNhaCungCap { get; set; }
        public required string DiaChi { get; set; }

        public required string SdtNhaCungCap { get; set; }

    }
}