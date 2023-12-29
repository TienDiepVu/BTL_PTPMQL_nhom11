using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models
{
    [Table("Thuoc")]
    public class Thuoc
    {
        [Key]
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; }
        public string NhomThuoc { get; set; }
        public string NhaCungCap { get; set; }
        public string CongDung { get; set; }        
    }
}