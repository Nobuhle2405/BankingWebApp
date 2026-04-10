using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public class Transfers
    {
        public int ts_id { get; set; }
        public int from_acc_id { get; set; }
        public int to_acc_id { get; set; }
        public int user_id { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
    }

    public class TransferRepository
    {
        private string connectionString;
        public TransferRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BankingDB1"].ConnectionString;
        }
        public List<Accounts> GetBalance(string username, string password)
        {
            List<Accounts> acclist = new List<Accounts>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT user_id FROM Users WHERE username = @UserName AND password = @Password ", conn);
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);
                object result = command.ExecuteScalar();

                if (result == null)
                {
                    throw new Exception("User not found.");
                }

                int userID = Convert.ToInt32(result);

                SqlCommand command1 = new SqlCommand("SELECT balance, acc_type FROM Accounts WHERE user_id = @user_id", conn);
                command1.Parameters.AddWithValue("@user_id", userID);
                command1.ExecuteNonQuery();
                SqlDataReader reader = command1.ExecuteReader();
                while (reader.Read())
                {
                    acclist.Add(new Accounts() 
                    {
                        balance = Convert.ToDecimal(reader["balance"]),
                        acc_type = reader["acc_type"].ToString()
                    });
                }
            }
            return acclist;
        }

        public void LoadUserTransfers(GridView gridView, string username, string password)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT user_id FROM Users WHERE username = @UserName AND password = @Password ", conn);
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);
                object result = command.ExecuteScalar();

                if (result == null)
                {
                    throw new Exception("User not found.");
                }

                int userID = Convert.ToInt32(result);

                SqlCommand command2 = new SqlCommand("SELECT * FROM Transfers WHERE user_id = @UserID", conn);
                command2.Parameters.AddWithValue("@UserID", userID);



                SqlDataReader reader = command2.ExecuteReader();
                gridView.DataSource = reader;
                gridView.DataBind();
            }
        }

        public decimal GetBalance1(int acc_id)
        {
            decimal balance = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT balance FROM Accounts WHERE acc_id = @acc_id", conn);
                command.Parameters.AddWithValue("@acc_id", acc_id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read() && reader["balance"] != DBNull.Value)
                    {
                        balance = Convert.ToDecimal(reader["balance"]);
                    }
                }
            }
            return balance;
        }

        public string AddTransfers(Transfers transfer, decimal amount, int accNum)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                decimal currentBalance = GetBalance1(accNum);

                using (var sqlTransaction = conn.BeginTransaction())
                {
                    if (currentBalance >= amount)
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand("INSERT INTO Transfers (from_acc_id, to_acc_id, user_id, amount, date) VALUES (@FAcc, @TAcc, @User, @Amount, @Date)", conn, sqlTransaction);
                            command.Parameters.AddWithValue("@FAcc", transfer.from_acc_id);
                            command.Parameters.AddWithValue("@TAcc", transfer.to_acc_id);
                            command.Parameters.AddWithValue("@User", transfer.user_id);
                            command.Parameters.AddWithValue("@Amount", transfer.amount);
                            command.Parameters.AddWithValue("@Date", transfer.date);
                            command.ExecuteNonQuery();


                            SqlCommand updateCommand = new SqlCommand("UPDATE Accounts SET balance = balance - @Amount WHERE acc_id = @FAccID", conn, sqlTransaction);
                            updateCommand.Parameters.AddWithValue("@Amount", transfer.amount);
                            updateCommand.Parameters.AddWithValue("@FAccID", transfer.from_acc_id);
                            updateCommand.ExecuteNonQuery();


                            SqlCommand updateCommand2 = new SqlCommand("UPDATE Accounts SET balance = balance + @Amount WHERE acc_id = @TAccID", conn, sqlTransaction);
                            updateCommand2.Parameters.AddWithValue("@Amount", transfer.amount);
                            updateCommand2.Parameters.AddWithValue("@TAccID", transfer.to_acc_id);
                            updateCommand2.ExecuteNonQuery();

                            message = "Transfer successful!";
                            sqlTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                            throw new Exception("Error occured while adding transfer: " + ex.Message, ex);
                        }
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        message = "Insufficient funds.";
                    }
                }
                
            }
            return message;
        }

        public int GetAccID(string acc_type, string username, string password)//get result 
        {
            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {

                conn.Open();
                SqlCommand command = new SqlCommand("SELECT user_id FROM Users WHERE username = @UserName AND password = @Password ", conn);
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);
                object result = command.ExecuteScalar();

                if (result == null)
                {
                    throw new Exception("User not found.");
                }

                int userID = Convert.ToInt32(result);

                SqlCommand command2 = new SqlCommand("SELECT acc_id FROM Accounts WHERE user_id = @User AND acc_type = @Type", conn);
                command2.Parameters.AddWithValue("@User", userID);
                command2.Parameters.AddWithValue("@Type", acc_type);
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

    public partial class Transfer : System.Web.UI.Page
    {

        private void BindTransGrid(string userName, string password)
        {
            TransferRepository transRep = new TransferRepository();

            transRep.LoadUserTransfers(gvTransfer, userName, password);

        }

        private void BindBalance()
        {
            string userName = Session["Username"]?.ToString();
            string password = Session["Password"]?.ToString();
            TransferRepository transfer = new TransferRepository();
            List<Accounts> accounts = transfer.GetBalance(userName, password);
            gvBalance.DataSource = accounts;
            gvBalance.DataBind();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userName = Session["Username"]?.ToString();
                string password = Session["Password"]?.ToString();
                if (userName != null && password != null)
                {
                    BindTransGrid(userName, password);
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnTransactions_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserTransactions.aspx");
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            string userName = Session["Username"]?.ToString();
            string password = Session["Password"]?.ToString();
            int userID = Convert.ToInt32(Session["UserID"]);
            TransferRepository transfer = new TransferRepository();

            Transfers newTransfers = new Transfers
            {
                from_acc_id = transfer.GetAccID(ddlFAccType.SelectedValue, userName, password),
                to_acc_id = transfer.GetAccID(ddlTAccType.SelectedValue, userName, password),
                user_id = userID,
                amount = Convert.ToDecimal(txtAmount.Text),
                date = DateTime.Now
            };
           

            try
            {
                lblMessage.Text = transfer.AddTransfers(newTransfers, Convert.ToDecimal(txtAmount.Text), transfer.GetAccID(ddlFAccType.SelectedValue, userName, password));
                transfer.LoadUserTransfers(gvTransfer, userName, password);
                BindTransGrid(userName, password);
                List<Accounts> accounts = transfer.GetBalance(userName, password);
                gvBalance.DataSource = accounts;
                gvBalance.DataBind();

                txtAmount.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }

        protected void btnBalance_Click(object sender, EventArgs e)
        {
            BindBalance();
        }
    }
}