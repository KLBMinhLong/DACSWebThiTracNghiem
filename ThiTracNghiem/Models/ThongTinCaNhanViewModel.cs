using System.ComponentModel.DataAnnotations;

public class ThongTinCaNhanViewModel
{
    public string TenTaiKhoan { get; set; } = "";

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    public DateTime NgaySinh { get; set; } 

    [Required(ErrorMessage = "Giới tính không được để trống")]
    public string GioiTinh { get; set; } = "";

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    public string SoDienThoai { get; set; } = "";

    [DataType(DataType.Password)]
    public string? MatKhauMoi { get; set; }

    [DataType(DataType.Password)]
    [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu không khớp")]
    public string? NhapLaiMatKhauMoi { get; set; }
}
