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
    public partial class ExportLHForm : Form
    {
        DataGridViewRow r;
        private QLSVDatabaseDataContext db;

        private int row = 0;
        private string dir;

        public ExportLHForm()
        {
            InitializeComponent();
        }

        private void ExportLHForm_Load(object sender, EventArgs e)
        {
            db = new QLSVDatabaseDataContext();
            ShowData();

            dgvBaoCao.Columns["MALP"].Width = 100;
            dgvBaoCao.Columns["TENLP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvBaoCao.Columns["NK"].Width = 100;

            dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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
            dgvBaoCao.DataSource = rs;
            row = rs.Count();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            var homNay = DateTime.Now;

            Document baoCao = new Document("Template\\Mau_Bao_Cao_Lop.doc");

            baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { homNay.ToString() });

            Table bangThongTinGiaDinh = baoCao.GetChild(NodeType.Table, 1, true) as Table;//Lấy bảng thứ 2 trong file mẫu
            int hangHienTai = 1;
            bangThongTinGiaDinh.InsertRows(hangHienTai, hangHienTai, row);
            for (int i = 1; i <= row; i++)
            {
                r = dgvBaoCao.Rows[hangHienTai-1];

                bangThongTinGiaDinh.PutValue(hangHienTai, 0, r.Cells["MALP"].Value.ToString());//Cột STT
                bangThongTinGiaDinh.PutValue(hangHienTai, 1, r.Cells["TENLP"].Value.ToString());//Cột Họ và tên
                bangThongTinGiaDinh.PutValue(hangHienTai, 2, r.Cells["NK"].Value.ToString());//Cột quan hệ
                hangHienTai++;
            }

            //Bước 4: Lưu và mở file
            try
            {
                baoCao.Save("BaoCaoLopHoc.docx");
                string temp = Application.StartupPath.ToString() + "/BaoCaoLopHoc.docx";
                //string destinationFilePath = Path.Combine(dir, Path.GetFileName(temp));
                //File.Copy(temp, destinationFilePath, true);
                string sourceFilePath = temp;
                string outputFilePath = dir + "/BaoCaoLopHoc.pdf";

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
            // Đọc file Word
            Document doc = new Document(sourceFilePath);

            // Chuyển đổi sang PDF
            doc.Save(outputFilePath, SaveFormat.Pdf);
        }
    }
}
