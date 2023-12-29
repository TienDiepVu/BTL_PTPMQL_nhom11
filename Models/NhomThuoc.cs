using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models 
{
    [Table("NhomThuoc")]
    public class NhomThuoc 
    {
        [Key]
        public required string MaNhomThuoc { get; set; }
        public required string TenNhomThuoc { get; set; }
        public string MoTa { get; set; }
    }
}