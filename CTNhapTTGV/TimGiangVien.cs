using System;
using System.Windows.Forms;

namespace CTNhapTTGV
{
    public partial class frmTimGiangVien : Form
    {
        private QuanLyGiangVien qlGiangVien;

        public frmTimGiangVien(QuanLyGiangVien ql)
        {
            InitializeComponent();
            qlGiangVien = ql;

            rdMaGV.Checked = true;
        }



        // So sánh theo mã giảng viên
        private int SoSanhMa(object a, object b)
        {
            GiangVien gv = a as GiangVien;
            string ma = b as string;

            return string.Compare(gv.MaSo, ma, true);
        }

        // So sánh theo họ tên
        private int SoSanhHoTen(object a, object b)
        {
            GiangVien gv = a as GiangVien;
            string hoTen = b as string;

            return string.Compare(gv.HoTen, hoTen, true);
        }

        // So sánh theo số điện thoại
        private int SoSanhSDT(object a, object b)
        {
            GiangVien gv = a as GiangVien;
            string soDT = b as string;

            return string.Compare(gv.SoDT, soDT, true);
        }

        private void btnTim_Click_1(object sender, EventArgs e)
        {
            string tuKhoa = txtTim.Text.Trim();

            GiangVien gv = null;

            if (rdMaGV.Checked)
            {
                gv = qlGiangVien.Tim(tuKhoa, SoSanhMa);
            }
            else if (rdHoTen.Checked)
            {
                gv = qlGiangVien.Tim(tuKhoa, SoSanhHoTen);
            }
            else if (rdSoDienThoai.Checked)
            {
                gv = qlGiangVien.Tim(tuKhoa, SoSanhSDT);
            }

            if (gv != null)
            {
                frmTBGiangVien frm = new frmTBGiangVien();
                frm.SetText(gv.ToString());
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show(
                    "Không tìm thấy thông tin!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }
    }
}