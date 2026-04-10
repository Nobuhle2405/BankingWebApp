using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Runtime.Remoting.Messaging;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApp
{
    public class Customers
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }

    public class CustomerRepository
    {


        private string connectionString;
        public CustomerRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BankingDB1"].ConnectionString;
        }

        public List<Customers> GetCustomers()
        {
            List<Customers> custlist = new List<Customers>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Users", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    custlist.Add(new Customers()
                    {
                        user_id = Convert.ToInt32(reader["user_id"]),
                        username = reader["username"].ToString(),
                        password = reader["password"].ToString(),
                        email = reader["email"].ToString()
                    });
                }
            }
            return custlist;
        }

        public bool CustomerExists(string custName, string custPassword)
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE username = @Name AND password = @Pass", conn);
                    command.Parameters.AddWithValue("@Name", custName);
                    command.Parameters.AddWithValue ("@Pass", custPassword);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting transaction: " + ex.Message);
                }
            };
        }

        public string AddCustomer(Customers customer, string custName, string custPass)
        {
            if (CustomerExists(custName, custPass))
            {
                return "Customer with the same name and password already exists. Retry."; 
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO Users (username, password, email) VALUES (@Name, @Password, @Email)", conn);
                    command.Parameters.AddWithValue("@Name", customer.username);
                    command.Parameters.AddWithValue("@Password", customer.password);
                    command.Parameters.AddWithValue("@Email", customer.email);
                    command.ExecuteNonQuery();
                }
                return "Customer added successfully!";
            }

        }

        public void UpdateCustomers(Customers customer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("UPDATE Users SET username = @Name, password = @Password, email = @Email WHERE user_id = @ID", conn);
                command.Parameters.AddWithValue("@ID", customer.user_id);
                command.Parameters.AddWithValue("@Name", customer.username);
                command.Parameters.AddWithValue("@Password", customer.password);
                command.Parameters.AddWithValue("@Email", customer.email);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCustomer(Customers customer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlTransaction transaction = conn.BeginTransaction();
                try
                {

                    SqlCommand command1 = new SqlCommand("DELETE FROM Transactions WHERE user_id = @user_id", conn, transaction);
                    command1.Parameters.AddWithValue("@user_id", customer.user_id);
                    command1.ExecuteNonQuery();


                    SqlCommand command2 = new SqlCommand("DELETE FROM Accounts WHERE user_id = @ID", conn, transaction);
                    command2.Parameters.AddWithValue("@ID", customer.user_id);
                    command2.ExecuteNonQuery();

                    SqlCommand command = new SqlCommand("DELETE FROM Users WHERE user_id = @ID", conn, transaction);
                    command.Parameters.AddWithValue("@ID", customer.user_id);
                    command.ExecuteNonQuery();


                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error deleting transaction: " + ex.Message);
                }


            }
        }
    }


    public partial class Customer : Page
    {
        private void BindCustomerGrid()
        {
            CustomerRepository customerRep = new CustomerRepository();
            List<Customers> customerList = customerRep.GetCustomers();
            gvCustomer.DataSource = customerList;
            gvCustomer.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomerGrid();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMessage.Text = "All fields are required. Please fill in Username, Email and Password.";
                return;
            }

            Customers newCustomer = new Customers
            {
                username = txtName.Text,
                email = txtEmail.Text,
                password = txtPassword.Text
            };

            CustomerRepository custRep = new CustomerRepository();

            lblMessage.Text = custRep.AddCustomer(newCustomer, txtName.Text, txtPassword.Text);

            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            BindCustomerGrid();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtName1.Text) ||
                string.IsNullOrWhiteSpace(txtEmail1.Text) ||
                string.IsNullOrWhiteSpace(txtPassword1.Text))
            {
                lblMessage.Text = "All fields are required for updating a customer.";
                return;
            }

            if (!int.TryParse(txtID.Text.Trim(), out int userId))
            {
                lblMessage.Text = "User ID must be a valid number.";
                return;
            }

            Customers updateCustomer = new Customers
            {
                user_id = userId, 
                username = txtName1.Text,
                email = txtEmail1.Text,
                password = txtPassword1.Text
            };

            CustomerRepository custRep = new CustomerRepository();

            custRep.UpdateCustomers(updateCustomer);

            txtID2.Text = string.Empty;
            txtName1.Text = string.Empty;
            txtEmail1.Text = string.Empty;
            txtPassword.Text = string.Empty;
            BindCustomerGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID2.Text))
            {
                lblMessage.Text = "Please enter a User ID to delete.";
                return;
            }

            if (!int.TryParse(txtID2.Text.Trim(), out int deleteId))
            {
                lblMessage.Text = "User ID must be a valid number.";
                return;
            }

            Customers deleteCustomer = new Customers
            {
                user_id = deleteId
            };

            CustomerRepository custRep = new CustomerRepository();

            custRep.DeleteCustomer(deleteCustomer);

            txtID2.Text = string.Empty;
            BindCustomerGrid();
        }
    }
}