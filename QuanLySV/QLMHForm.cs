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
    public partial class QLMHForm : Form
    {
        DataGridViewRow r;
        private QLSVDatabaseDataContext db;

        public QLMHForm()
        {
            InitializeComponent();
        }

        private void QLMHForm_Load(object sender, EventArgs e)
        {
            db = new QLSVDatabaseDataContext();
            ShowData();

            dgvMain.Columns["MAMH"].Width = 100;
            dgvMain.Columns["TENMH"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMain.Columns["SOTC"].Width = 100;
        }

        private void ShowData()
        {
            var rs = from s in db.MONHOCs
                     select new
                     {
                         s.MAMH,
                         s.TENMH,
                         s.SOTC
                     };
            dgvMain.DataSource = rs;
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

                txtMaMonHoc.Text = r.Cells["MAMH"].Value.ToString();
                txtTenMonHoc.Text = r.Cells["TENMH"].Value.ToString();
                txtSoTinChi.Text = r.Cells["SOTC"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaMonHoc.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã môn học!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaMonHoc.Select();
                    return;
                }
                if (string.IsNullOrEmpty(txtTenMonHoc.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên môn học!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenMonHoc.Select();
                    return;
                }
                if (string.IsNullOrEmpty(txtSoTinChi.Text))
                {
                    MessageBox.Show("Vui lòng nhập số tín chỉ!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoTinChi.Select();
                    return;
                }

                MONHOC l = new MONHOC();
                l.MAMH = txtMaMonHoc.Text;
                l.TENMH = txtTenMonHoc.Text;
                l.SOTC = Int32.Parse(txtSoTinChi.Text);
                db.MONHOCs.InsertOnSubmit(l);
                try
                {
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Thêm môn học thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    db.MONHOCs.DeleteOnSubmit(l);
                    ShowData();
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                //ShowData();
                //MessageBox.Show("Thêm môn học thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var l = db.MONHOCs.SingleOrDefault(x => x.MAMH == r.Cells["MAMH"].Value.ToString());
                db.MONHOCs.DeleteOnSubmit(l);
                db.SubmitChanges();
                MONHOC l2 = new MONHOC();
                l2.MAMH = txtMaMonHoc.Text;
                l2.TENMH = txtTenMonHoc.Text;
                l2.SOTC = Int32.Parse(txtSoTinChi.Text);
                db.MONHOCs.InsertOnSubmit(l2);
                try
                {
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Sửa môn học thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    db.MONHOCs.DeleteOnSubmit(l2);
                    ShowData();
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                //ShowData();
                //MessageBox.Show("Sửa môn học thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Vui lòng chọn môn học!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (
                MessageBox.Show("Bạn thực sự muốn xóa môn học " + r.Cells["TENMH"].Value.ToString() + " ?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                try
                {
                    var l = db.MONHOCs.SingleOrDefault(x => x.MAMH == r.Cells["MAMH"].Value.ToString());
                    db.MONHOCs.DeleteOnSubmit(l);
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
            txtMaMonHoc.Text = null;
            txtTenMonHoc.Text = null;
            txtSoTinChi.Text = null;
        }
    }
}
