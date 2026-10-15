using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CTQLSV_.frmTuyChon;

namespace CTQLSV_
{
    public partial class frmSinhVien : Form
    {
        QuanLySinhVien qlsv = new QuanLySinhVien();
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            qlsv.DocTuFile("DanhSachSV.txt");
            LoadListView();
        }
        private int SoSanhTheoMa(object sv1, object sv2)
        {
            SinhVien sv = sv2 as SinhVien;
            return sv.MaSo.CompareTo(sv1);
        }
        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = this.lvSinhVien.SelectedItems.Count;
            if (count > 0)
            {
                ListViewItem lvitem = this.lvSinhVien.SelectedItems[0];
                SinhVien sv = GetSinhVienLV(lvitem);
                ThietLapThongTin(sv);
            }
        }
        private void ThemSV(SinhVien sv)
        {
            ListViewItem lvitem = new ListViewItem(sv.MaSo);
            lvitem.SubItems.Add(sv.HoTen);
            lvitem.SubItems.Add(sv.NgaySinh.ToShortDateString());
            lvitem.SubItems.Add(sv.DiaChi);
            lvitem.SubItems.Add(sv.Lop);
            string gt = "Nữ";
            if (sv.GioiTinh)
                gt = "Nam";
            lvitem.SubItems.Add(gt);
            string cn = "";
            foreach (string s in sv.ChuyenNganh)
                cn += s + ",";
            cn = cn.Substring(0, cn.Length - 1);
            lvitem.SubItems.Add(cn);
            lvitem.SubItems.Add(sv.Hinh);
            this.lvSinhVien.Items.Add(lvitem);
        }
        //Hiển thị các sinh viên trong qlsv lên ListView
        private void LoadListView()
        {
            this.lvSinhVien.Items.Clear();
            foreach (SinhVien sv in qlsv.dsSinhVien)
            {
                ThemSV(sv);
            }
            CapNhatTongSinhVien();
        }

        // Phương thức lấy thông tin sinh viên từ thông tin nhập trên các control.
        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien();
            bool gt = true;
            List<string> cn = new List<string>();
            sv.MaSo = this.mtxtMaSo.Text;
            sv.HoTen = this.txtHoTen.Text;
            sv.NgaySinh = this.dtpNgaySinh.Value;
            sv.DiaChi = this.txtDiaChi.Text;
            sv.Lop = this.cboLop.Text;
            sv.Hinh = this.txtHinh.Text;
            if (rdNu.Checked)
                gt = false;
            sv.GioiTinh = gt;
            for (int i = 0; i < this.clbChuyenNganh.Items.Count; i++)
                if (clbChuyenNganh.GetItemChecked(i))
                    cn.Add(clbChuyenNganh.Items[i].ToString());
            sv.ChuyenNganh = cn;
            return sv;
        }

        // Phương thức lấy thông tin sinh viên từ item của ListView lvSinhVien.
        private SinhVien GetSinhVienLV(ListViewItem lvitem)
        {
            SinhVien sv = new SinhVien();
            sv.MaSo = lvitem.SubItems[0].Text;
            sv.HoTen = lvitem.SubItems[1].Text;
            sv.NgaySinh = DateTime.Parse(lvitem.SubItems[2].Text);
            sv.DiaChi = lvitem.SubItems[3].Text;
            sv.Lop = lvitem.SubItems[4].Text;
            sv.GioiTinh = false;
            if (lvitem.SubItems[5].Text == "Nam")
                sv.GioiTinh = true;
            List<string> cn = new List<string>();
            string[] s = lvitem.SubItems[6].Text.Split(',');
            foreach (string t in s)
                cn.Add(t);
            sv.ChuyenNganh = cn;
            sv.Hinh = lvitem.SubItems[7].Text;
            return sv;
        }
        // Phương thức thiết lập thông tin sinh viên lên control.
        private void ThietLapThongTin(SinhVien sv)
        {
            this.mtxtMaSo.Text = sv.MaSo;
            this.txtHoTen.Text = sv.HoTen;
            this.dtpNgaySinh.Value = sv.NgaySinh;
            this.txtDiaChi.Text = sv.DiaChi;
            this.cboLop.Text = sv.Lop;
            this.txtHinh.Text = sv.Hinh;
            HienThiHinh(sv.Hinh);
            if (sv.GioiTinh)
                this.rdNam.Checked = true;
            else
                this.rdNu.Checked = true;

            for (int i = 0; i < this.clbChuyenNganh.Items.Count; i++)
                this.clbChuyenNganh.SetItemChecked(i, false);

            foreach (string s in sv.ChuyenNganh)
            {
                for (int i = 0; i < this.clbChuyenNganh.Items.Count; i++)
                    if (s.CompareTo(this.clbChuyenNganh.Items[i]) == 0)
                        this.clbChuyenNganh.SetItemChecked(i, true);
            }
        }
        private void HienThiHinh(string tenHinh)
        {
            try
            {
                // Xóa hình cũ
                if (pbHinh.Image != null)
                {
                    pbHinh.Image.Dispose();
                    pbHinh.Image = null;
                }

                if (string.IsNullOrWhiteSpace(tenHinh))
                    return;

                tenHinh = tenHinh.Trim();

                // Lấy đường dẫn thư mục chạy chương trình
                string thuMucChay = AppDomain.CurrentDomain.BaseDirectory;

                // Các vị trí có thể chứa hình
                string[] danhSachDuongDan =
                {
            Path.Combine(thuMucChay, tenHinh),
            Path.Combine(Application.StartupPath, tenHinh),

            // Trường hợp ảnh nằm trong thư mục Images
            Path.Combine(thuMucChay, "Images", tenHinh),

            // Trường hợp ảnh nằm ở thư mục project
            Path.Combine(thuMucChay, "..", "..", tenHinh)
        };

                string duongDan = null;

                foreach (string path in danhSachDuongDan)
                {
                    string pathFull = Path.GetFullPath(path);

                    if (File.Exists(pathFull))
                    {
                        duongDan = pathFull;
                        break;
                    }
                }

                // Không tìm thấy
                if (duongDan == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy hình!\n\n" +
                        "Tên hình: " + tenHinh + "\n\n" +
                        "Thư mục chương trình:\n" + thuMucChay,
                        "Lỗi hình ảnh",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Đọc hình
                using (Image temp = Image.FromFile(duongDan))
                {
                    pbHinh.Image = new Bitmap(temp);
                }

                pbHinh.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                pbHinh.Image = null;

                MessageBox.Show(
                    "Không thể hiển thị hình!\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void groupboxTTSV_Enter(object sender, EventArgs e)
        {

        }

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            this.mtxtMaSo.Text = "";
            this.txtHoTen.Text = "";
            this.dtpNgaySinh.Value = DateTime.Now;
            this.txtDiaChi.Text = "";
            this.cboLop.Text = this.cboLop.Items[0].ToString();
            this.txtHinh.Text = "";

            if (this.pbHinh.Image != null)
            {
                this.pbHinh.Image.Dispose();
                this.pbHinh.Image = null;
            }

            this.rdNam.Checked = true;

            for (int i = 0; i < this.clbChuyenNganh.Items.Count; i++)
                this.clbChuyenNganh.SetItemChecked(i, false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int count, i;
            ListViewItem lvitem;
            count = this.lvSinhVien.Items.Count - 1;
            for (i = count; i >= 0; i--)
            {
                lvitem = this.lvSinhVien.Items[i];
                if (lvitem.Checked)
                    qlsv.Xoa(lvitem.SubItems[0].Text, SoSanhTheoMa);
            }
            this.LoadListView();
            this.btnMacDinh.PerformClick();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            bool kqsua;
            kqsua = qlsv.Sua(sv, sv.MaSo, SoSanhTheoMa);
            if (kqsua)
            {
                this.LoadListView();
            }
        }

        private void pbHinh_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Title = "Open File Image";
            openFile.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png|All File|*.*";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Hiển thị hình vừa chọn
                    using (Image temp = Image.FromFile(openFile.FileName))
                    {
                        pbHinh.Image = new Bitmap(temp);
                    }

                    pbHinh.SizeMode = PictureBoxSizeMode.Zoom;

                    // Lưu tên hình vào txtHinh
                    txtHinh.Text = Path.GetFileName(openFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Không thể mở hình!\n" + ex.Message,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void CapNhatTongSinhVien()
        {
            lblTongSV.Text = "Tổng Sinh Viên: " + lvSinhVien.Items.Count;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();

            if (string.IsNullOrWhiteSpace(sv.MaSo))
            {
                MessageBox.Show(
                    "Vui lòng nhập mã số sinh viên!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                mtxtMaSo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(sv.HoTen))
            {
                MessageBox.Show(
                    "Vui lòng nhập họ tên sinh viên!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtHoTen.Focus();
                return;
            }

            // Kiểm tra mã số đã tồn tại chưa
            SinhVien svTrung = qlsv.Tim(sv.MaSo, SoSanhTheoMa);

            if (svTrung != null)
            {
                MessageBox.Show(
                    "Mã số sinh viên đã tồn tại!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                mtxtMaSo.Focus();
                return;
            }

            // Thêm sinh viên
            qlsv.Them(sv);

            // Hiển thị lại danh sách
            LoadListView();

            MessageBox.Show(
                "Thêm sinh viên thành công!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Xóa trắng các ô nhập
            btnMacDinh.PerformClick();
        }

        private void mnuMoFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Title = "Open File Image";
            openFile.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png|All File|*.*";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (Image temp = Image.FromFile(openFile.FileName))
                    {
                        pbHinh.Image = new Bitmap(temp);
                    }

                    pbHinh.SizeMode = PictureBoxSizeMode.Zoom;
                    txtHinh.Text = Path.GetFileName(openFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở hình!\n" + ex.Message);
                }
            }
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuThem_Click(object sender, EventArgs e)
        {
            btnThem.PerformClick();
        }

        private void mnuXoa_Click(object sender, EventArgs e)
        {
            btnXoa.PerformClick();
        }

        private void mnuSua_Click(object sender, EventArgs e)
        {
            btnSua.PerformClick();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();

            fd.Font = lvSinhVien.Font;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                lvSinhVien.Font = fd.Font;
            }
        }

        private void màuChữToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            cd.Color = lvSinhVien.ForeColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                lvSinhVien.ForeColor = cd.Color;
            }
        }

        private void sắpXếpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTuyChon f = new frmTuyChon();

            if (f.ShowDialog() == DialogResult.OK)
            {
                switch (f.KieuSapXep)
                {
                    case KieuTuyChon.MaSV:
                        qlsv.dsSinhVien.Sort((sv1, sv2) =>
                            sv1.MaSo.CompareTo(sv2.MaSo));
                        break;

                    case KieuTuyChon.HoTen:
                        qlsv.dsSinhVien.Sort((sv1, sv2) =>
                            sv1.HoTen.CompareTo(sv2.HoTen));
                        break;

                    case KieuTuyChon.NgaySinh:
                        qlsv.dsSinhVien.Sort((sv1, sv2) =>
                            sv1.NgaySinh.CompareTo(sv2.NgaySinh));
                        break;
                }

                LoadListView();
            }
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTuyChon frm = new frmTuyChon();

            if (frm.ShowDialog() == DialogResult.Yes)
            {
                string thongTin = frm.ThongTinTim;
                bool timThay = false;

                foreach (ListViewItem item in lvSinhVien.Items)
                {
                    string giaTriCanTim = "";

                    if (frm.KieuSapXep == frmTuyChon.KieuTuyChon.MaSV)
                    {
                        giaTriCanTim = item.SubItems[0].Text;
                    }
                    else if (frm.KieuSapXep == frmTuyChon.KieuTuyChon.HoTen)
                    {
                        giaTriCanTim = item.SubItems[1].Text;
                    }
                    else if (frm.KieuSapXep == frmTuyChon.KieuTuyChon.NgaySinh)
                    {
                        giaTriCanTim = item.SubItems[2].Text;
                    }

                    if (giaTriCanTim.Equals(
                        thongTin,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        // Chọn sinh viên tìm thấy
                        item.Selected = true;
                        item.Focused = true;
                        item.EnsureVisible();

                        // Hiển thị thông tin lên các control
                        SinhVien sv = GetSinhVienLV(item);
                        ThietLapThongTin(sv);

                        timThay = true;
                        break;
                    }
                }

                if (!timThay)
                {
                    MessageBox.Show(
                        "Không tìm thấy sinh viên!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void mnuThem_Click_1(object sender, EventArgs e)
        {
            btnThem.PerformClick();
        }

        private void mnuMoFile_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Title = "Open File Image";
            openFile.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png|All File|*.*";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (Image temp = Image.FromFile(openFile.FileName))
                    {
                        pbHinh.Image = new Bitmap(temp);
                    }

                    pbHinh.SizeMode = PictureBoxSizeMode.Zoom;
                    txtHinh.Text = Path.GetFileName(openFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở hình!\n" + ex.Message);
                }
            }
        }

        private void mnuThoat_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuXoa_Click_1(object sender, EventArgs e)
        {
            btnXoa.PerformClick();
        }

        private void mnuSua_Click_1(object sender, EventArgs e)
        {
            btnSua.PerformClick();
        }
    }
}
