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
    public partial class QLLHForm : Form
    {
        DataGridViewRow r;
        private QLSVDatabaseDataContext db;

        public QLLHForm()
        {
            InitializeComponent();
        }

        private void QLLHForm_Load(object sender, EventArgs e)
        {
            db = new QLSVDatabaseDataContext();
            ShowData();

            dgvMain.Columns["MALP"].Width = 100;
            dgvMain.Columns["TENLP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMain.Columns["NK"].Width = 100;
        }

        private void ShowData()
        {
            var rs = from s in db.LOPs
                     select new
                     {
                         s.MALP,
                         s.TENLP,
                         s.NK
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

                txtMaLop.Text = r.Cells["MALP"].Value.ToString();
                txtTenLop.Text = r.Cells["TENLP"].Value.ToString();
                txtNienKhoa.Text = r.Cells["NK"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaLop.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã lớp!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaLop.Select();
                    return;
                }
                if (string.IsNullOrEmpty(txtTenLop.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên lớp!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenLop.Select();
                    return;
                }
                if (string.IsNullOrEmpty(txtNienKhoa.Text))
                {
                    MessageBox.Show("Vui lòng nhập niên khóa!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNienKhoa.Select();
                    return;
                }

                LOP l = new LOP();
                l.MALP = txtMaLop.Text;
                l.TENLP = txtTenLop.Text;
                l.NK = txtNienKhoa.Text;
                db.LOPs.InsertOnSubmit(l);
                try
                {
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Thêm lớp học thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    db.LOPs.DeleteOnSubmit(l);
                    ShowData();
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            ResetField();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn loại phòng!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var l = db.LOPs.SingleOrDefault(x => x.MALP == r.Cells["MALP"].Value.ToString());
                db.LOPs.DeleteOnSubmit(l);
                db.SubmitChanges();
                LOP l2 = new LOP();
                l2.MALP = txtMaLop.Text;
                l2.TENLP = txtTenLop.Text;
                l2.NK = txtNienKhoa.Text;
                db.LOPs.InsertOnSubmit(l2);
                try
                {
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Sửa lớp học thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    db.LOPs.DeleteOnSubmit(l2);
                    ShowData();
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            ResetField();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Vui lòng chọn lớp học!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (
                MessageBox.Show("Bạn thực sự muốn xóa lớp học " + r.Cells["TENLP"].Value.ToString() + " ?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                try
                {
                    var l = db.LOPs.SingleOrDefault(x => x.MALP == r.Cells["MALP"].Value.ToString());
                    db.LOPs.DeleteOnSubmit(l);
                    db.SubmitChanges();
                    ShowData();
                    MessageBox.Show("Xóa thành công!", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }

                ResetField();
            }
        }

        private void ResetField()
        {
            r = null;
            txtMaLop.Text = null;
            txtTenLop.Text = null;
            txtNienKhoa.Text = null;
        }
    }
}
