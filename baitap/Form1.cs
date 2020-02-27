using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace baitap
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Thoát chương trình
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Muốn đi à?", "Ờ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialog == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        // Hiển thị mật khẩu
        private void checkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkShowPass.Checked == true)
            {
                txtPass.PasswordChar = (char)0;
            } else
            {
                txtPass.PasswordChar = '$';
            }
        }

        // Đăng nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        // Đăng nhập
        public void login()
        {
            var client = new RestClient("https://api.user.dulieutnmt.vn:8989/api/v1.0/user/login");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("account", txtID.Text);
            request.AddParameter("pass", txtPass.Text);

            // Convert sang object
            IRestResponse response = client.Execute(request);
            dynamic json = JsonConvert.DeserializeObject(response.Content);
 
            if (json.data.ToString() != "success")
            {
                MessageBox.Show("Nhập sai rồi", "Cyka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                this.Hide();
                frmSub frm = new frmSub();
                frm.Show();
            }
        }
    }
}
