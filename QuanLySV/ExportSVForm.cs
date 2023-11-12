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
using Aspose.Words;
using ThuVienWinform.Report.AsposeWordExtension;
using Aspose.Words.Tables;
using System.IO;

namespace QuanLySV
{
    public partial class ExportSVForm : Form
    {
        DataGridViewRow r;
        private QLSVDatabaseDataContext db;

        private int row = 0;
        private string dir;

        public ExportSVForm()
        {
            InitializeComponent();
        }

        private void ExportSVForm_Load(object sender, EventArgs e)
        {
            db = new QLSVDatabaseDataContext();
            ShowData();

            dgvBaoCao.Columns["MASV"].Width = 100;
            dgvBaoCao.Columns["TENSV"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvBaoCao.Columns["DCSV"].Width = 100;
            dgvBaoCao.Columns["MALP"].Width = 50;

            dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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
            dgvBaoCao.DataSource = rs;
            row = rs.Count();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            var homNay = DateTime.Now;

            Document baoCao = new Document("Template\\Mau_Bao_Cao_SinhVien.doc");

            baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { homNay.ToString() });

            Table bangThongTinGiaDinh = baoCao.GetChild(NodeType.Table, 1, true) as Table;
            int hangHienTai = 1;
            bangThongTinGiaDinh.InsertRows(hangHienTai, hangHienTai, row);
            for (int i = 1; i <= row; i++)
            {
                r = dgvBaoCao.Rows[hangHienTai - 1];

                bangThongTinGiaDinh.PutValue(hangHienTai, 0, r.Cells["MASV"].Value.ToString());
                bangThongTinGiaDinh.PutValue(hangHienTai, 1, r.Cells["TENSV"].Value.ToString());
                bangThongTinGiaDinh.PutValue(hangHienTai, 2, r.Cells["DCSV"].Value.ToString());
                bangThongTinGiaDinh.PutValue(hangHienTai, 3, r.Cells["MALP"].Value.ToString());
                hangHienTai++;
            }

            try
            {
                baoCao.Save("BaoCaoSinhVien.docx");
                string temp = Application.StartupPath.ToString() + "/BaoCaoSinhVien.docx";
                string sourceFilePath = temp;
                string outputFilePath = dir + "/BaoCaoSinhVien.pdf";

                ConvertToPdf(sourceFilePath, outputFilePath);
                System.Diagnostics.Process.Start(outputFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ConvertToPdf(string sourceFilePath, string outputFilePath)
        {
            Document doc = new Document(sourceFilePath);

            doc.Save(outputFilePath, SaveFormat.Pdf);
        }
    }
}
