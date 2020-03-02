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
using Newtonsoft.Json;
using Console = Colorful.Console;

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
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            import();
        }

        public void show()
        {
            string filePath = openFileDialog1.FileName;
            string extension = Path.GetExtension(filePath);
            string conStr, sheetName, sheetName2;

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
                    sheetName = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                    sheetName2 = dtExcelSchema.Rows[3]["TABLE_NAME"].ToString();
                    
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

                        DataTable dt2 = new DataTable();
                        cmd.CommandText = "SELECT * From [" + sheetName2 + "]";
                        cmd.Connection = con;
                        con.Open();
                        oda.SelectCommand = cmd;
                        oda.Fill(dt);
                        con.Close();

                        dt.Merge(dt2);

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

            List<Document> documents = new List<Document>();
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            
            for (int i = 4; i <= rowCount; i++)
            {
                Document document = new Document();

                void addAttribute(int index, string att)
                {
                    if (xlRange.Cells[i, index].Value2 == null)
                    {
                        att = "";
                        //console.writeline("shit");
                    }
                    else
                    {
                        att = xlRange.Cells[i, index].Value2.ToString();
                        //Console.WriteLine(att);
                    }
                }

                void addAttributeInt(int index, int att)
                {
                    if (xlRange.Cells[i, index].Value2 == null)
                    {
                        att = 0;
                        //console.writeline("shit");
                    }
                    else
                    {
                        att = int.Parse(xlRange.Cells[i, index].Value2.ToString());
                        //Console.WriteLine(att);
                    }
                }

                // Identifier
                addAttribute(1, document.Identifier);

                //if (xlRange.Cells[i, 1].Value2 == null)
                //{
                //    document.Identifier = "";
                //    //console.writeline("shit");
                //}
                //else
                //{
                //    document.Identifier = xlRange.Cells[i, 1].Value2.ToString();
                //    //console.writeline(document.Identifier);
                //}

                // Title
                addAttribute(2, document.Title);

                // Document Type
                addAttribute(3, document.DocumentType);

                // Location ID **
                string id = "";
                var clientLocation = new RestClient("https://api.kho.dulieutnmt.vn:8443/api/v1.0/location/");
                clientLocation.Timeout = -1;
                var requestLocation = new RestRequest(Method.GET);
                requestLocation.AddHeader("access_token", frmMain.access_token);
                requestLocation.AddHeader("app_id", "h0VA9ACBnBswmGyaFMngbKAYAtMa");
                IRestResponse responseLocation = clientLocation.Execute(requestLocation);

                dynamic json = JsonConvert.DeserializeObject(responseLocation.Content);


                if (xlRange.Cells[i, 7].Value2 == null || xlRange.Cells[i, 6].Value2 == null || xlRange.Cells[i, 5].Value2 == null || xlRange.Cells[i, 4].Value2 == null)
                {
                    document.LocationId = "";
                }

                bool CheckLocation = false;

                //for (int j = 0; j < json.data.Count; j++)
                //{
                //    if (xlRange.Cells[i, 7].Value2 == json.data[i].Name.ToString())
                //    {
                //        Console.WriteLine("ok");
                //    }
                //}



                Console.WriteLine(document.LocationId);

                // Don vi hanh chinh ID

                addAttributeInt(9, document.DonViHanhChinhId);
                //if (xlRange.Cells[i, 9].Value2 == null)
                //{
                //    document.DonViHanhChinhId = 0;
                //    //console.writeline("shit");
                //}
                //else
                //{
                //    document.DonViHanhChinhId = int.Parse(xlRange.Cells[i, 9].Value2.ToString());
                //    //console.writeline(document.DonViHanhChinhId);
                //}

                // Contributor
                addAttribute(10, document.Contributor);

                // Coverage
                addAttribute(11, document.Coverage);

                // Creator
                addAttribute(12, document.Creator);

                // Department ID
                addAttribute(13, document.DepartmentId);

                // Date **
                //if (xlRange.Cells[i, 14].Value2 == null)
                //{
                //    document.Date = null;
                //    //console.writeline("shit");
                //}
                //else
                //{
                //    document.Date = xlRange.Cells[i, 14].Value2.ToString();
                //    //console.writeline(document.Date);
                //}

                // Date Type
                addAttributeInt(15, document.DateType);

                //Description
                addAttribute(16, document.Description);

                // Language
                addAttribute(17, document.Language);

                // Publisher
                addAttribute(18, document.Publisher);

                // Relations **

                // Rights
                addAttribute(20, document.Rights);

                // Source
                addAttribute(21, document.Source);

                // Subject
                addAttribute(22, document.Subject);

                // Amount
                addAttributeInt(23, document.Amount);

                // Unit
                addAttribute(24, document.Unit);

                // Security level
                switch (xlRange.Cells[i, 25].Value2) {
                    case null:
                        document.SecurityLevel = 0;
                        //console.writeline("shit");
                        break;
                    case "Không mật":
                        document.SecurityLevel = 0;
                        //console.writeline("shit");
                        break;
                    case "Bình thường":
                        document.SecurityLevel = 1;
                        //console.writeline("shit");
                        break;
                    case "Mật":
                        document.SecurityLevel = 2;
                        //console.writeline("shit");
                        break;
                    case "Tối mật":
                        document.SecurityLevel = 3;
                        //console.writeline("shit");
                        break;
                    default:
                        break;
                }

                // Is Free
                if (xlRange.Cells[i, 26].Value2 == null)
                {
                    document.IsFree = false;
                    //console.writeline("shit");
                }
                else if (xlRange.Cells[i, 26].Value2 == "Miễn phí")
                {
                    document.IsFree = true;
                    //console.writeline(document.IsFree);
                } else
                {
                    document.IsFree = false;
                    //console.writeline(document.IsFree);
                } 


                documents.Add(document);
            }

            foreach (Document doc in documents)
            {
                Console.WriteLine(doc.Title);
            }
            

            // Components
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet3 = xlWorkbook.Sheets[3];
            Microsoft.Office.Interop.Excel.Range xlRange3 = xlWorksheet.UsedRange;

            List<Component> components = new List<Component>();
            int rowCount3 = xlRange3.Rows.Count;
            int colCount3 = xlRange3.Columns.Count;

            for (int i = 4; i <= rowCount; i++)
            {
                Component component = new Component();

                void addAttribute(int index, string att)
                {
                    if (xlRange3.Cells[i, index].Value2 == null)
                    {
                        att = "";
                        //console.writeline("shit");
                    }
                    else
                    {
                        att = xlRange3.Cells[i, index].Value2.ToString();
                        //Console.WriteLine(att);
                    }
                }

                // Id

                // Title
                addAttribute(2, component.Title);

                components.Add(component);
            }


            //    var client = new RestClient("https://api.kho.dulieutnmt.vn:8443/api/v1.0/document/");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json", documents ,ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            ////console.writeline(response.Content);
            ///
            var jsonUp = JsonConvert.SerializeObject(documents);

            var client = new RestClient("https://api.kho.dulieutnmt.vn:8443/api/v1.0/document");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", frmMain.access_token);
            request.AddHeader("app_id", "h0VA9ACBnBswmGyaFMngbKAYAtMa");
            request.AddHeader("Authorization", "Basic eVRfNmphajBvZXJXZHNoOEZRdU5oVUNreGZrYTp3OWJsVWdja0c1NUtfMlo0cEprNUFpYXVJOVVh");
            request.AddParameter("application/json", jsonUp, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

        }

        
    }
}
