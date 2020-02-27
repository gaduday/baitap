using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using RestSharp;

namespace baitap
{
    public partial class frmSub : Form
    {
        public frmSub()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                openFileDialog1.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
        private string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            show();
            import();
        }

        public void show()
        {
            string filePath = openFileDialog1.FileName;
            string extension = Path.GetExtension(filePath);
            string conStr, sheetName;

            conStr = string.Empty;
            switch (extension)
            {

                case ".xls": //Excel 97-03
                    conStr = string.Format(Excel03ConString, filePath, 0);
                    break;

                case ".xlsx": //Excel 07
                    conStr = string.Format(Excel07ConString, filePath, 0);
                    break;
            }

            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    int i = 0;
                    sheetName = dtExcelSchema.Rows[i]["TABLE_NAME"].ToString();
                    con.Close();
                }
            }

            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter())
                    {
                        DataTable dt = new DataTable();
                        cmd.CommandText = "SELECT * From [" + sheetName + "]";
                        cmd.Connection = con;
                        con.Open();
                        oda.SelectCommand = cmd;
                        oda.Fill(dt);
                        con.Close();

                        //Populate DataGridView.
                        dataGridView1.DataSource = dt;
                    }
                }
            }          
        }

        public void import()
        {
            string filePath = openFileDialog1.FileName;

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            List<Excel> listEx = new List<Excel>();
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            for (int i = 2; i <= rowCount; i++)
            {
                Excel excel = new Excel();

                if (xlRange.Cells[i, 2].Value2 == null)
                {
                    excel.Title = "";
                } else
                {
                    excel.Title = xlRange.Cells[i, 2].Value2.ToString();
                }

                if (xlRange.Cells[i, 3].Value2 == null)
                {
                    excel.DocumentType = "";
                }
                else
                {
                    excel.DocumentType = xlRange.Cells[i, 3].Value2.ToString();
                }

                if (xlRange.Cells[i, 4].Value2 == null)
                {
                    excel.LocationID = "";
                }
                else
                {
                    excel.LocationID = xlRange.Cells[i, 4].Value2.ToString();
                }

                if (xlRange.Cells[i, 5].Value2 == null)
                {
                    excel.Creator = "";
                }
                else
                {
                    excel.Creator = xlRange.Cells[i, 5].Value2.ToString();
                }

                if (xlRange.Cells[i, 6].Value2 == null)
                {
                    excel.Date = "";
                }
                else
                {
                    excel.Date = xlRange.Cells[i, 6].Value2.ToString();
                }

                if (xlRange.Cells[i, 7].Value2 == null)
                {
                    excel.Description = "";
                }
                else
                {
                    excel.Description = xlRange.Cells[i, 7].Value2.ToString();
                }
                
                listEx.Add(excel);
            }

            var client = new RestClient("https://api.kho.dulieutnmt.vn:8443/api/v1.0/document/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", listEx ,ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

        }
    }
}
