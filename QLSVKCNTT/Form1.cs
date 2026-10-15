using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QLSVKCNTT
{
    public partial class Form1 : Form
    {
        private QuanLySinhVien qlsv;
        private bool isModified = false; //dùng để cảnh báo lưu file khi đóng Form
        private string filePath = "DSSV.txt";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            DocFileDSSV(filePath);
            HienThiDanhSach();
            isModified = false;
        }
        private void HienThiDanhSach()
        {
            HienThiDanhSach(qlsv.DanhSach);
        }

        private void HienThiDanhSach(ArrayList danhSach)
        {
            lvSinhVien.Items.Clear();

            foreach (SinhVien sv in danhSach)
            {
                ListViewItem item = new ListViewItem(sv.MSSV);
                item.SubItems.Add(sv.HoTen);
                item.SubItems.Add(sv.Phai ? "Nam" : "Nữ");
                item.SubItems.Add(sv.NgaySinh.ToString("dd/MM/yyyy"));
                item.SubItems.Add(sv.Lop);
                item.SubItems.Add(sv.SoDienThoai);
                item.SubItems.Add(sv.Email);
                item.SubItems.Add(sv.DiaChi);
                item.SubItems.Add(sv.Hinh);

                lvSinhVien.Items.Add(item);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSinhVien.SelectedItems.Count > 0)
            {
                // Lấy dòng đang được chọn đầu tiên
                ListViewItem item = lvSinhVien.SelectedItems[0];
                string mssv = item.SubItems[0].Text;

                // Tìm thông tin đầy đủ từ danh sách
                SinhVien sv = qlsv.TimTheoMSSV(mssv);
                if (sv != null)
                {
                    //Đổ dữ liệu vào giao diện 
                    mtbMSSV.Text = sv.MSSV;
                    txtHoTen.Text = sv.HoTen;
                    if (sv.Phai) rdNam.Checked = true; else rdNu.Checked = true;
                    dtpNgaySinh.Value = sv.NgaySinh;
                    cboLop.SelectedItem = sv.Lop;
                    mtbSDT.Text = sv.SoDienThoai;
                    txtEmail.Text = sv.Email;
                    txtDiaChi.Text = sv.DiaChi;
                    txtHinh.Text = sv.Hinh;

                    // Nạp hình ảnh

                    txtHinh.Text = sv.Hinh;

                    // Lấy đường dẫn tuyệt đối chính xác tới thư mục bin\Debug
                    string fullImagePath = Path.Combine(Application.StartupPath, sv.Hinh);

                    if (!string.IsNullOrEmpty(sv.Hinh) && File.Exists(fullImagePath))
                    //tên 0 rỗng, file tồn tại trên ổ cứng -> nạp ảnh
                    {
                        if (picHinh.Image != null)
                            picHinh.Image.Dispose();// Giải phóng ảnh cũ (nếu có) để tránh khóa file

                        // Đọc ảnh qua Stream để không khóa file và nạp đúng từ bin\Debug
                        using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(fullImagePath))) 
                        // Đọc dữ liệu vào một mảng byte trong RAM rồi đóng file trên ổ cứng ngay lập tức 
                        // Memory chuyển mảng byte đó thành luồng dữ liệu trong bộ nhớ RAM
                        {
                            picHinh.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        if (picHinh.Image != null) picHinh.Image.Dispose();
                        picHinh.Image = null;
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Chuẩn hóa & kiểm tra MSSV (loại bỏ khoảng trắng thừa từ MaskedTextBox)
            string mssv = mtbMSSV.Text.Replace(" ", "").Trim();
            if (mssv.Length < 7)
            {
                MessageBox.Show("Mã sinh viên phải đủ 7 số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbMSSV.Focus();
                return;
            }

            // 2. Kiểm tra Họ tên không được bỏ trống
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            // 3. Lấy tên lớp an toàn (tránh Crash khi cboLop chưa chọn Item)
            string tenLop = cboLop.SelectedItem != null ? cboLop.SelectedItem.ToString() : cboLop.Text;

            // 4. Đóng gói đối tượng SinhVien (dùng mssv đã chuẩn hóa)
            SinhVien sv = new SinhVien(
                mssv,
                txtHoTen.Text.Trim(),
                rdNam.Checked,
                dtpNgaySinh.Value,
                tenLop,
                mtbSDT.Text.Trim(),
                txtEmail.Text.Trim(),
                txtDiaChi.Text.Trim(),
                txtHinh.Text.Trim()
            );

            // 5. Kiểm tra tồn tại để phân nhánh Thêm mới / Cập nhật
            SinhVien svTim = qlsv.TimTheoMSSV(sv.MSSV);
            if (svTim != null)
            {
                qlsv.CapNhat(sv); // Đã tồn tại -> Cập nhật thông tin
            }
            else
            {
                qlsv.Them(sv);   // Chưa có -> Thêm sinh viên mới
            }

            // 6. Cập nhật trạng thái và hiển thị lại ListView
            isModified = true;
            HienThiDanhSach();
            MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void btnChonHinh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; // Chỉ chọn file ảnh
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtHinh.Text = dlg.FileName; // Lưu đường dẫn

                    // Giải phóng ảnh cũ (nếu có) để tránh khóa file
                    if (picHinh.Image != null) picHinh.Image.Dispose();
                    picHinh.Image = Image.FromFile(dlg.FileName); // Hiển thị ảnh mới
                }
            }
        }

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            mtbMSSV.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtHinh.Clear();
            mtbSDT.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            rdNam.Checked = true;
            cboLop.SelectedIndex = 0;

            if (picHinh.Image != null)
            {
                picHinh.Image.Dispose();
                picHinh.Image = null;
            }
            mtbMSSV.Focus(); // Đưa con trỏ về ô MSSV
        }
        private void DocFileDSSV(string path)
        {
            // 1. Kiểm tra tồn tại file
           if (!File.Exists(path))
            {
                MessageBox.Show("Khong ton tai file");
                return;
            }
            qlsv.DanhSach.Clear();
            string[] lines = File.ReadAllLines(path);
            int soDongThanhCong = 0;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                string[] p = line.Split('\t');
                if (p.Length < 9)
                {
                    p = System.Text.RegularExpressions.Regex.Split(line, @"\t+ | \s{2,}");
                }

                if (p.Length >= 8)
                {
                    DateTime ngaySinh;
                    if (!DateTime.TryParseExact(p[3].Trim(), "dd/MM/yyyy",
                        null, System.Globalization.DateTimeStyles.None, out ngaySinh))
                    {
                        DateTime.TryParse(p[3].Trim(), out ngaySinh);
                    }



                    string mssv = p[0].Trim();
                    string hoTen = p[1].Trim();
                    bool gioiTinh = p[2].Trim() == "1" || p[2].Trim().ToLower() == "nam";
                    string lop = p[4].Trim();
                    string sdt = p[5].Trim();
                    string email = p[6].Trim();
                    string diaChi = p[7].Trim();
                    string hinh = p.Length >= 9 ? p[8].Trim() : ""; // Nếu thiếu hình thì để rỗng

                    SinhVien sv = new SinhVien(mssv, hoTen, gioiTinh, ngaySinh, lop, sdt, email, diaChi, hinh);
                    qlsv.Them(sv);
                    soDongThanhCong++;
                }
            }

            // 2. Thông báo kết quả để kiểm tra
            if (soDongThanhCong == 0 && lines.Length > 0)
            {
                MessageBox.Show($"Đã đọc được {lines.Length} dòng trong file nhưng KHÔNG định dạng được dòng nào!\nLỗi: Do định dạng khoảng cách giữa các cột trong file DSNV.txt không đúng.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuXoa_Click(object sender, EventArgs e)
        {
            if (lvSinhVien.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Xóa các sinh viên đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Duyệt qua tất cả các dòng đang được bôi đen để xóa
                    foreach (ListViewItem item in lvSinhVien.SelectedItems)
                    {
                        string mssv = item.SubItems[0].Text;
                        qlsv.Xoa(mssv);
                    }
                    isModified = true;
                    HienThiDanhSach();
                    btnMacDinh_Click(sender, e);
                }
            }
        }

        private void mnuTaiLaiDS_Click(object sender, EventArgs e)
        {
            DocFileDSSV(filePath);
            HienThiDanhSach();
            isModified = false; // Reset lại cờ
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isModified)
            {
                DialogResult result = MessageBox.Show(
                    "Danh sách sinh viên đã bị thay đổi. Bạn có muốn lưu vào tập tin DSSV.txt không?",
                    "Xác nhận lưu",
                    MessageBoxButtons.YesNoCancel, // Chuyển sang YesNoCancel
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    GhiFileDSSV(filePath); // Đồng ý -> Lưu rồi thoát
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // Bấm Cancel -> Hủy lệnh đóng Form
                }
            }
        }

        // Hàm bổ trợ ghi dữ liệu ra File text
        private void GhiFileDSSV(string path)
        {
            List<string> lines = new List<string>();
            foreach (SinhVien sv in qlsv.DanhSach)
            {
                // Định dạng mỗi sinh viên thành 1 dòng, các cột cách nhau bằng dấu Tab (\t)
                string line = $"{sv.MSSV}\t{sv.HoTen}\t{(sv.Phai ? "1" : "0")}\t{sv.NgaySinh:dd/MM/yyyy}\t{sv.Lop}\t{sv.SoDienThoai}\t{sv.Email}\t{sv.DiaChi}\t{sv.Hinh}";
                lines.Add(line);
            }
            File.WriteAllLines(path, lines);
        }
    }
}
