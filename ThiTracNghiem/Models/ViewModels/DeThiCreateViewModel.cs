using System.ComponentModel.DataAnnotations;

public class DeThiCreateViewModel
{
    [Required]
    public string TenDeThi { get; set; }

    [Required]
    [Range(1, 300)]
    public int ThoiGianLamBai { get; set; }

    [Required]
    public int ChuDeId { get; set; }

    public List<int> CauHoiIds { get; set; } = new();
}
