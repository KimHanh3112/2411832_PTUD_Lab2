using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSVKCNTT
{
    public class QuanLySinhVien
    {
        public ArrayList DanhSach { get; set; }
        public QuanLySinhVien()
        {
            DanhSach = new ArrayList();
        }
        public void Them(SinhVien sv)
        {
            DanhSach.Add(sv);
        }
        public SinhVien TimTheoMSSV (string mssv)
        {
            foreach (SinhVien sv in DanhSach)
            {
                if (sv.MSSV == mssv)
                    return sv;
            }
            return null;
        }
        public bool CapNhat (SinhVien svSua)
        {
            for (int i = 0; i < DanhSach.Count; i++)
            {
                SinhVien sv = (SinhVien)DanhSach[i];
                if (sv.MSSV == svSua.MSSV)
                {
                    DanhSach[i] = svSua;
                    return true;
                }
            }
            return false;
        }
        public bool Xoa (string mssv)
        {
            for (int i = 0; i < DanhSach.Count; i++)
            {
                SinhVien sv = (SinhVien)DanhSach[i];
                if (sv.MSSV == mssv)
                {
                    DanhSach.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}
