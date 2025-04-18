public class ChiTietDeThi
{
    public int Id { get; set; }

    public int DeThiId { get; set; }
    public DeThi? DeThi { get; set; }

    public int CauHoiId { get; set; }
    public CauHoi? CauHoi { get; set; }
}
