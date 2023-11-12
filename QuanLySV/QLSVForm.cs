using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySV.DB;

namespace QuanLySV
{
    public partial class QLSVForm : Form
    {
        DataGridViewRow r;
        private QLSVDatabaseDataContext db;

        public QLSVForm()
        {
            InitializeComponent();
        }

        private void QLSVForm_Load(object sender, EventArgs e)
        {
            db = new QLSVDatabaseDataContext();
            ShowData();

            dgvMain.Columns["MASV"].Width = 100;
            dgvMain.Columns["TENSV"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMain.Columns["DCSV"].Width = 100;
            dgvMain.Columns["MALP"].Width = 50;
        }

        private void ShowData()
        {
            var rs = from s in db.SINHVIENs
                     select new
                     {
                         s.MASV,
                         s.TENSV,
                         s.DCSV,
                         s.MALP
                     };
            dgvMain.DataSource = rs;

            cbbMaLop.Items.Clear();

            var rs2 = from s in db.LOPs
                     select new
                     {
                         s.MALP
                     };

            foreach (var p in rs2)
            {
                cbbMaLop.Items.Add(p.MALP);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvMain.Rows[e.RowIndex];

                txtMaSinhVien.Text = r.Cells["MASV"].Value.ToString();
                txtTenSinhVien.Text = r.Cells["TENSV"].Value.ToString();
                txtDiaChi.Text = r.Cells["DCSV"].Value.ToString();
                cbbMaLop.Text = r.Cells["MALP"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaSinhVien.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã sinh viên!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaSinhVien.Select();
                    return;
                }
                if (string.IsNullOrEmpty(txtTenSinhVien.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên sinh viên!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenSinhVien.Select();
                    return;
                }
                if (string.IsNullOrEmpty(txtDiaChi.Text))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiaChi.Select();
                    return;
                }
                if (string.IsNullOrEmpty(cbbMaLop.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã lớp!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbMaLop.Select();
                    return;
                }

                SINHVIEN l = new SINHVIEN();
                l.MASV = txtMaSinhVien.Text;
                l.TENSV = txtTenSinhVien.Text;
                l.DCSV = txtDiaChi.Text;
                l.MALP = cbbMaLop.Text;
                db.SINHVIENs.InsertOnSubmit(l);
                try
                {
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Thêm sinh viên thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    db.SINHVIENs.DeleteOnSubmit(l);
                    ShowData();
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {

                ShowData();
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            ResetField();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn môn học!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var l = db.SINHVIENs.SingleOrDefault(x => x.MASV == r.Cells["MASV"].Value.ToString());
                db.SINHVIENs.DeleteOnSubmit(l);
                db.SubmitChanges();
                SINHVIEN l2 = new SINHVIEN();
                l2.MASV = txtMaSinhVien.Text;
                l2.TENSV = txtTenSinhVien.Text;
                l2.DCSV = txtDiaChi.Text;
                l2.MALP = cbbMaLop.Text;
                db.SINHVIENs.InsertOnSubmit(l2);
                try
                {
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Sửa sinh viên thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    db.SINHVIENs.DeleteOnSubmit(l2);
                    ShowData();
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                ShowData();
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            ResetField();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (
                MessageBox.Show("Bạn thực sự muốn xóa sinh viên " + r.Cells["TENSV"].Value.ToString() + " ?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                try
                {
                    var l = db.SINHVIENs.SingleOrDefault(x => x.MASV == r.Cells["MASV"].Value.ToString());
                    db.SINHVIENs.DeleteOnSubmit(l);
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Xóa thành công!", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    ShowData();
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }

                ResetField();
            }
        }

        private void ResetField()
        {
            r = null;
            txtMaSinhVien.Text = null;
            txtTenSinhVien.Text = null;
            txtDiaChi.Text = null;
            cbbMaLop.Text = null;
        }
    }
}
