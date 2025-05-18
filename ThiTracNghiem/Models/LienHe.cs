using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ThiTracNghiem.Models
{
    public class LienHe
    {
        public int Id { get; set; }

        [Required]
        public string TenTaiKhoan { get; set; }

        [ForeignKey("TenTaiKhoan")]
        public TaiKhoan? TaiKhoan { get; set; }

        [Required]
        [MaxLength(100)]
        public string HoTen { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(1000)]
        public string NoiDung { get; set; }

        public DateTime NgayGui { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string TrangThaiXuLy { get; set; } = "Chưa xử lý"; // hoặc "Đã xử lý"
    }
}
