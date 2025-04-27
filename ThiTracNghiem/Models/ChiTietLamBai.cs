using System.ComponentModel.DataAnnotations;

namespace ThiTracNghiem.Models
{
    public class ChiTietLamBai
    {
        public int Id { get; set; }

        public int LichSuLamBaiId { get; set; }
        public LichSuLamBai? LichSuLamBai { get; set; }

        public int CauHoiId { get; set; }
        public CauHoi? CauHoi { get; set; }

        public string? DapAnChon { get; set; } // A/B/C/D
        public bool? DungHaySai { get; set; } // True nếu đúng
    }
}