using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySV
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private Form currentFormChild;

        private void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }

            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void quảnLýLớpHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLLHForm());
        }

        private void quảnLýMônHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLMHForm());
        }

        private void quảnLýSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLSVForm());
        }

        private void quảnLýĐiểmSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLDForm());
        }

        private void báoCáoLớpHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportLHForm eQLL = new ExportLHForm();
            eQLL.Show();
        }

        private void hỗTrợToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/profile.php?id=100011461034984");
        }

        private void báoCáoMônHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportMHForm eQLMH = new ExportMHForm();
            eQLMH.Show();
        }

        private void báoCáoSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportSVForm eQLSV = new ExportSVForm();
            eQLSV.Show();
        }

        private void báoCáoMônHọcToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExportDForm eQLD = new ExportDForm();
            eQLD.Show();
        }
    }
}
