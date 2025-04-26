using System.ComponentModel.DataAnnotations;

public class DeThiCreateViewModel
{
    [Required]
    public string TenDeThi { get; set; }

    [Required]
    [Range(1, 300)]
    public int ThoiGianLamBai { get; set; }

    public bool TrangThaiMo { get; set; } = true;
    
    [Required]
    public int ChuDeId { get; set; }

}
