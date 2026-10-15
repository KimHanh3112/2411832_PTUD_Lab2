using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTQLSV_
{
    public partial class frmTuyChon : Form
    {
        public enum KieuTuyChon
        {
            MaSV,
            HoTen,
            NgaySinh
        }
        public KieuTuyChon KieuSapXep { get; private set; }
        public string ThongTinTim { get; private set; }
        public frmTuyChon()
        {
            InitializeComponent();
            rdMaSo.Checked = true;
        }

        private void rdMaSo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmTuyChon_Load(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSapXep_Click(object sender, EventArgs e)
        {
            if (rdMaSo.Checked)
            {
                KieuSapXep = KieuTuyChon.MaSV;
            }
            else if (rdHoTen.Checked)
            {
                KieuSapXep = KieuTuyChon.HoTen;
            }
            else if (rdNgaySinh.Checked)
            {
                KieuSapXep = KieuTuyChon.NgaySinh;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (rdMaSo.Checked)
            {
                KieuSapXep = KieuTuyChon.MaSV;
            }
            else if (rdHoTen.Checked)
            {
                KieuSapXep = KieuTuyChon.HoTen;
            }
            else if (rdNgaySinh.Checked)
            {
                KieuSapXep = KieuTuyChon.NgaySinh;
            }

            // Lấy thông tin cần tìm
            ThongTinTim = txtTimThongTin.Text.Trim();

            // Kiểm tra người dùng có nhập hay chưa
            if (string.IsNullOrWhiteSpace(ThongTinTim))
            {
                MessageBox.Show(
                    "Vui lòng nhập thông tin cần tìm!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTimThongTin.Focus();
                return;
            }

            // Trả kết quả về frmSinhVien
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
