using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public class Reg
    {
        private string connectionString;
        public Reg()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BankingDB1"].ConnectionString;
        }

        public int GetUserID(string name, string pass)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                SqlCommand command2 = new SqlCommand("SELECT user_id FROM Users WHERE username = @Name AND password = @Pass ", conn);
                command2.Parameters.AddWithValue("@Name", name);
                command2.Parameters.AddWithValue("@Pass", pass);
                object result2 = command2.ExecuteScalar();

                if (result2 == null)
                {
                    throw new Exception("User not found.");
                }

                int id = Convert.ToInt32(result2);
                return id;
            }

        }
    }
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMessage.Text = "All fields are required. Please fill in Username, Email and Password.";
                lblMessage.ForeColor = System.Drawing.Color.DeepPink;
                return;
            }

            Customers newCustomer = new Customers
            {
                username = txtName.Text,
                email = txtEmail.Text,
                password = txtPassword.Text
            };

            CustomerRepository custRep = new CustomerRepository();

            string result = custRep.AddCustomer(newCustomer, txtName.Text, txtPassword.Text);

            if (result != "Customer added successfully!")
            {
                lblMessage.Text = result;
                lblMessage.ForeColor = System.Drawing.Color.DeepPink;
                return;
            }

            Reg reg = new Reg();

            Accounts newAccount = new Accounts
            {
                user_id = reg.GetUserID(txtName.Text, txtPassword.Text),
                acc_type = "Cheque"
            };

            AccountRepository accRep = new AccountRepository();
            accRep.AddAccounts(newAccount);

            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            lblMessage.Text = "Registration successful! Please LOG OUT";
        }
    }
}