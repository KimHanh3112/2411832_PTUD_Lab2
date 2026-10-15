using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTQLSV_
{
    public delegate int SoSanh(object sv1, object sv2);
    internal class QuanLySinhVien
    {
        public List<SinhVien> dsSinhVien;
        public QuanLySinhVien()
        {
            dsSinhVien = new List<SinhVien>();
        }
        public SinhVien this[int index]
        {
            get { return this.dsSinhVien[index]; }
            set { dsSinhVien[index] = value; }
        }
        public void Them(SinhVien sv)
        {
            dsSinhVien.Add(sv);
        }

        public SinhVien Tim(object obj, SoSanh ss)
        {
            SinhVien svresult = null;
            foreach (SinhVien sv in dsSinhVien)
                if (ss(obj, sv) == 0)
                {
                    svresult = sv;
                    break;
                }
            return svresult;
        }

        public bool Sua(SinhVien svsua, object obj, SoSanh ss)
        {
            for (int i = 0; i < dsSinhVien.Count; i++)
            {
                if (ss(obj, dsSinhVien[i]) == 0)
                {
                    dsSinhVien[i] = svsua;
                    return true;
                }
            }

            return false;
        }

        public void Xoa(object obj, SoSanh ss)
        {
            int i = dsSinhVien.Count - 1;
            for (; i >= 0; i--)
                if (ss(obj, this.dsSinhVien[i]) == 0)
                    dsSinhVien.RemoveAt(i);
        }

        public void DocTuFile(string filename)
        {
            string t;
            string[] s;
            SinhVien sv;

            using (StreamReader sr = new StreamReader(
                new FileStream(filename, FileMode.Open)))
            {
                while ((t = sr.ReadLine()) != null)
                {
                    // Bỏ qua dòng trống
                    if (string.IsNullOrWhiteSpace(t))
                        continue;

                    // File sử dụng TAB để ngăn cách các trường
                    s = t.Split('\t');

                    // Mỗi sinh viên phải có 8 trường
                    if (s.Length < 8)
                    {
                        MessageBox.Show(
                            "Dòng dữ liệu không đúng định dạng:\n\n" +
                            t +
                            "\n\nSố trường đọc được: " + s.Length,
                            "Lỗi dữ liệu",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        continue;
                    }

                    sv = new SinhVien();

                    // 0. Mã số
                    sv.MaSo = s[0].Trim();

                    // 1. Họ tên
                    sv.HoTen = s[1].Trim();

                    // 2. Ngày sinh
                    sv.NgaySinh = DateTime.ParseExact(
                        s[2].Trim(),
                        "M/d/yyyy",
                        CultureInfo.InvariantCulture);

                    // 3. Địa chỉ
                    sv.DiaChi = s[3].Trim();

                    // 4. Lớp
                    sv.Lop = s[4].Trim();

                    // 5. Hình
                    sv.Hinh = s[5].Trim();

                    // 6. Giới tính
                    // 1 = Nam, 0 = Nữ
                    sv.GioiTinh = s[6].Trim() == "1";

                    // 7. Chuyên ngành
                    string[] dsChuyenNganh = s[7].Split(',');

                    foreach (string cn in dsChuyenNganh)
                    {
                        if (!string.IsNullOrWhiteSpace(cn))
                        {
                            sv.ChuyenNganh.Add(cn.Trim());
                        }
                    }

                    // Thêm sinh viên vào danh sách
                    this.Them(sv);
                }
            }
        }
    }
}
