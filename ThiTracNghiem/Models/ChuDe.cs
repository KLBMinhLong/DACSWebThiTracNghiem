using System.ComponentModel.DataAnnotations;

public class ChuDe
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên chủ đề không được để trống")]
    public string TenChuDe { get; set; }

    public ICollection<CauHoi>? CacCauHoi { get; set; }
}
