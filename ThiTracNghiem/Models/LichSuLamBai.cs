using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThiTracNghiem.Models
{
    public class LichSuLamBai
    {
        public int Id { get; set; }

        [Required]
        public string TenTaiKhoan { get; set; } = "";

        [ForeignKey("TenTaiKhoan")]
        public TaiKhoan? TaiKhoan { get; set; }


        public int DeThiId { get; set; }
        public DeThi? DeThi { get; set; }

        public DateTime NgayBatDau { get; set; } = DateTime.Now;
        public DateTime? NgayNopBai { get; set; }

        public double? Diem { get; set; }

        public ICollection<ChiTietLamBai>? ChiTietLamBais { get; set; }
    }
}