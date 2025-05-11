using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThiTracNghiem.Models
{
    public class ChiTietCauTraLoi
    {
        public string CauHoi { get; set; }
        public string HinhAnhUrl { get; set; }
        public string AudioUrl { get; set; }
        public string DapAnA { get; set; }
        public string DapAnB { get; set; }
        public string DapAnC { get; set; }
        public string DapAnD { get; set; }

        public string DapAnChon { get; set; }
        public string DapAnDung { get; set; }
        public bool DungHaySai { get; set; }
    }


    public class KetQuaViewModel
    {
        public int TongCauHoi { get; set; }
        public int SoCauDung { get; set; }
        public double DiemSo { get; set; }
        public List<ChiTietCauTraLoi> ChiTietTraLoi { get; set; }
    }
}
