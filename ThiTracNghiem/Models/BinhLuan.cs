using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThiTracNghiem.Models
{
    public class BinhLuan
    {
        public int Id { get; set; }

        [Required]
        public string TenTaiKhoan { get; set; } = "";

        [ForeignKey("TenTaiKhoan")]
        public TaiKhoan? TaiKhoan { get; set; }

        [Required]
        public int DeThiId { get; set; }

        [ForeignKey("DeThiId")]
        public DeThi? DeThi { get; set; }

        [Required]
        [StringLength(500)]
        public string NoiDung { get; set; } = "";

        public DateTime ThoiGian { get; set; } = DateTime.Now;
    }
}