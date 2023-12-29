using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models {
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string ChucVu { get; set; }
        public string MucLuong { get; set; }

        public string SoDienThoai { get; set; }
    }
}