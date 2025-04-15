using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CauHoi
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nội dung câu hỏi không được để trống")]
    public string NoiDung { get; set; } = "";

    [Required]
    public string DapAnA { get; set; } = "";

    [Required]
    public string DapAnB { get; set; } = "";

    [Required]
    public string DapAnC { get; set; } = "";

    [Required]
    public string DapAnD { get; set; } = "";

    [Required]
    [RegularExpression("^[ABCD]$", ErrorMessage = "Chỉ được chọn A, B, C hoặc D")]
    public string DapAnDung { get; set; } = "";

    // Khóa ngoại
    [Required(ErrorMessage = "Phải chọn chủ đề")]
    public int ChuDeId { get; set; }

    [ForeignKey("ChuDeId")]
    public ChuDe? ChuDe { get; set; }
}
