using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DeThi
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên đề thi không được để trống")]
    public string TenDeThi { get; set; } = "";

    [Range(1, 300, ErrorMessage = "Thời gian làm bài phải từ 1 đến 300 phút")]
    public int ThoiGianLamBai { get; set; }

    public DateTime NgayTao { get; set; } = DateTime.Now;

    public bool TrangThaiMo { get; set; } = true;

    [Required]
    public int ChuDeId { get; set; }

    [ForeignKey("ChuDeId")]
    public ChuDe? ChuDe { get; set; }

    public int SoLuongCauHoi { get; set; } // Số lượng câu cần lấy khi làm bài

    // public ICollection<ChiTietDeThi> ChiTietDeThis { get; set; } = new List<ChiTietDeThi>();
    public ICollection<ChiTietDeThi>? ChiTietCauHoi { get; set; }
}
