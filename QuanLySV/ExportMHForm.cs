﻿using System;
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
    public partial class ExportMHForm : Form
    {
        DataGridViewRow r;
        private QLSVDatabaseDataContext db;

        private int row = 0;
        private string dir;

        public ExportMHForm()
        {
            InitializeComponent();
        }

        private void ExportMHForm_Load(object sender, EventArgs e)
        {
            db = new QLSVDatabaseDataContext();
            ShowData();

            dgvBaoCao.Columns["MAMH"].Width = 100;
            dgvBaoCao.Columns["TENMH"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvBaoCao.Columns["SOTC"].Width = 100;

            dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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
            dgvBaoCao.DataSource = rs;
            row = rs.Count();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            var homNay = DateTime.Now;

            Document baoCao = new Document("Template\\Mau_Bao_Cao_MonHoc.doc");

            baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { homNay.ToString() });

            Table bangThongTinGiaDinh = baoCao.GetChild(NodeType.Table, 1, true) as Table;
            int hangHienTai = 1;
            bangThongTinGiaDinh.InsertRows(hangHienTai, hangHienTai, row);
            for (int i = 1; i <= row; i++)
            {
                r = dgvBaoCao.Rows[hangHienTai - 1];

                bangThongTinGiaDinh.PutValue(hangHienTai, 0, r.Cells["MAMH"].Value.ToString());
                bangThongTinGiaDinh.PutValue(hangHienTai, 1, r.Cells["TENMH"].Value.ToString());
                bangThongTinGiaDinh.PutValue(hangHienTai, 2, r.Cells["SOTC"].Value.ToString());
                hangHienTai++;
            }

            try
            {
                baoCao.Save("BaoCaoMonHoc.docx");
                string temp = Application.StartupPath.ToString() + "/BaoCaoMonHoc.docx";
                string sourceFilePath = temp;
                string outputFilePath = dir + "/BaoCaoMonHoc.pdf";

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
