using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;

namespace WebApp
{


    public partial class Login : System.Web.UI.Page
    {
        private string connectionString;
        public Login()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BankingDB1"].ConnectionString;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = this.Master as SiteMaster;
            if (master != null)
            {
                master.ShowNavigationBar = false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT user_id FROM Users WHERE username = @Username AND password = @Password", conn);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int user_id  = Convert.ToInt32(reader["user_id"]);
                    reader.Close();

                    SqlCommand command1 = new SqlCommand("SELECT acc_id FROM Accounts WHERE user_id = @UserID", conn);
                    command1.Parameters.AddWithValue("@UserID", user_id);

                    object result = command1.ExecuteScalar();
                    if (result != null)
                    {
                        int acc_id = Convert.ToInt32(result);

                        Session["Username"] = username;
                        Session["Password"] = password;
                        Session["AccID"] = acc_id;
                        Session["UserID"] = user_id;
                    }
                    else
                    {
                        lblMessage.Text = "No account found for this user.";
                    }


                    FormsAuthentication.SetAuthCookie(username, false);
                    if (password == "admin123" && username == "Admin")
                        Response.Redirect("~/Customer.aspx");
                    else
                        Response.Redirect("~/UserTransactions.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid username or password.";
                }


            }

            txtPassword.Text = string.Empty;
            txtUsername.Text = string.Empty;
        }
    }
}
    