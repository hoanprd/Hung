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
    public partial class QLDForm : Form
    {
        DataGridViewRow r;
        private QLSVDatabaseDataContext db;

        public QLDForm()
        {
            InitializeComponent();
        }

        private void QLDForm_Load(object sender, EventArgs e)
        {
            db = new QLSVDatabaseDataContext();
            ShowData();

            dgvMain.Columns["MASV"].Width = 100;
            dgvMain.Columns["MAMH"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMain.Columns["DIEM"].Width = 100;
        }

        private void ShowData()
        {
            var rs = from s in db.DIEMSVs
                     select new
                     {
                         s.MASV,
                         s.MAMH,
                         s.DIEM
                     };
            dgvMain.DataSource = rs;

            cbbMaSinhVien.Items.Clear();
            cbbMaMonHoc.Items.Clear();

            var rs2 = from s in db.SINHVIENs
                      select new
                      {
                          s.MASV
                      };

            foreach (var p in rs2)
            {
                cbbMaSinhVien.Items.Add(p.MASV);
            }

            var rs3 = from s in db.MONHOCs
                      select new
                      {
                          s.MAMH
                      };

            foreach (var p in rs3)
            {
                cbbMaMonHoc.Items.Add(p.MAMH);
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

                cbbMaSinhVien.Text = r.Cells["MASV"].Value.ToString();
                cbbMaMonHoc.Text = r.Cells["MAMH"].Value.ToString();
                txtDiem.Text = r.Cells["DIEM"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbbMaSinhVien.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã sinh viên!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbMaSinhVien.Select();
                    return;
                }
                if (string.IsNullOrEmpty(cbbMaMonHoc.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã môn học!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbMaMonHoc.Select();
                    return;
                }
                if (string.IsNullOrEmpty(txtDiem.Text))
                {
                    MessageBox.Show("Vui lòng nhập điểm!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiem.Select();
                    return;
                }

                DIEMSV l = new DIEMSV();
                l.MASV = cbbMaSinhVien.Text;
                l.MAMH = cbbMaMonHoc.Text;
                l.DIEM = Convert.ToDouble(txtDiem.Text);
                db.DIEMSVs.InsertOnSubmit(l);
                try
                {
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Thêm sinh viên thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    db.DIEMSVs.DeleteOnSubmit(l);
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
                MessageBox.Show("Vui lòng chọn sinh viên!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var l = db.DIEMSVs.Where(x => x.MASV == r.Cells["MASV"].Value.ToString() && x.MAMH == r.Cells["MAMH"].Value.ToString()).FirstOrDefault();
                db.DIEMSVs.DeleteOnSubmit(l);
                db.SubmitChanges();
                DIEMSV l2 = new DIEMSV();
                l2.MASV = cbbMaSinhVien.Text;
                l2.MAMH = cbbMaMonHoc.Text;
                l2.DIEM = Convert.ToDouble(txtDiem.Text);
                db.DIEMSVs.InsertOnSubmit(l2);
                try
                {
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Sửa điểm sinh viên thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    db.DIEMSVs.DeleteOnSubmit(l2);
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
                MessageBox.Show("Bạn thực sự muốn xóa điểm sinh viên có mã là " + r.Cells["MASV"].Value.ToString() + " và có mã môn học là " + r.Cells["MAMH"].Value.ToString() + " ?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                try
                {
                    var l = db.DIEMSVs.Where(x => x.MASV == r.Cells["MASV"].Value.ToString() && x.MAMH == r.Cells["MAMH"].Value.ToString()).FirstOrDefault();
                    db.DIEMSVs.DeleteOnSubmit(l);
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
            cbbMaSinhVien.Text = null;
            cbbMaMonHoc.Text = null;
            txtDiem.Text = null;
        }

        private void txtDiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
