using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThiTracNghiem.Models
{
    public class LamBaiViewModel
    {
        public DeThi DeThi { get; set; }
        public List<CauHoi> CauHois { get; set; }
    }
}
