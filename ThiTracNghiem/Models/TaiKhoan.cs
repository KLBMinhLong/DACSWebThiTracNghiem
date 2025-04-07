using System;
using System.ComponentModel.DataAnnotations;

namespace ThiTracNghiem.Models
{
    public class TaiKhoan
    {
        [Key]
        [Required]
        public string TenTaiKhoan { get; set; } = string.Empty;

        [Required]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime NgaySinh { get; set; }

        [Required]
        public string GioiTinh { get; set; } = string.Empty;

        [Required]
        public string SoDienThoai { get; set; } = string.Empty;

        public string VaiTro { get; set; } = "hocvien";
        public bool TrangThaiKhoa { get; set; } = false;

        public DateTime? ThoiGianTao { get; set; } = DateTime.Now;
        public string? AnhDaiDienUrl { get; set; }
    }
}
