using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSVKCNTT
{
    public class SinhVien
    {
        public string MSSV { get; set; }
        public string HoTen { get; set; }
        public bool Phai { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Lop { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string Hinh { get; set; }

        public SinhVien() { }

        public SinhVien ( string mssv, string hoTen, bool phai, DateTime ngaySinh, 
                          string lop, string soDienThoai, string email, string diaChi, string hinh)
        {
            MSSV = mssv;
            HoTen = hoTen;
            Phai = phai;
            NgaySinh = ngaySinh;
            Lop = lop;
            SoDienThoai = soDienThoai;
            Email = email;
            DiaChi = diaChi;
            Hinh = hinh;
        }
    }
}
