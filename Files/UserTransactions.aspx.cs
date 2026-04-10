using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public class ExtTransactions
    {
        public int et_id { get; set; }
        public int from_acc_id { get; set; }
        public int from_user_id { get; set; }
        public int to_acc_id { get; set; }
        public int to_user_id { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
    }


    public class UserTransaction
    {
        private string connectionString;
        public UserTransaction()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BankingDB1"].ConnectionString;
        }
        public List<Accounts> GetBalance(int acc_id)
        {
            List<Accounts> acclist = new List<Accounts>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT balance FROM Accounts WHERE acc_id = @acc_id", conn);
                command.Parameters.AddWithValue("@acc_id", acc_id);
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    acclist.Add(new Accounts()
                    {
                        balance = Convert.ToDecimal(reader["balance"])
                    });
                }
            }
            return acclist;
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


        public void LoadUserTransactions(GridView gridView, string username, string password)
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

                SqlCommand command2 = new SqlCommand("SELECT * FROM Transactions WHERE user_id = @UserID", conn);
                command2.Parameters.AddWithValue("@UserID", userID);
                command2.ExecuteNonQuery();

                SqlDataReader reader = command2.ExecuteReader();
                gridView.DataSource = reader;
                gridView.DataBind();
            }
        }

        public void LoadExtTransactions(GridView gridView, string username, string password)
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

                SqlCommand command2 = new SqlCommand("SELECT * FROM ExtTransactions WHERE from_user_id = @UserID OR to_user_id = @UserID", conn);
                command2.Parameters.AddWithValue("@UserID", userID);

                SqlDataReader reader = command2.ExecuteReader();
                gridView.DataSource = reader;
                gridView.DataBind();
            }
        }


        public string AddTransaction(Transactions transaction, decimal amount, int accNum)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                decimal currentBalance = GetBalance1(accNum);

                using (var sqlTransaction = conn.BeginTransaction())
                {
                    
                        try
                        {



                            if (transaction.trans_type == "Withdrawal")
                            {
                                if (currentBalance >= amount)
                                {
                                    SqlCommand command = new SqlCommand("INSERT INTO Transactions (acc_id, user_id, amount, date,trans_type) VALUES (@Acc, @User, @Amount, @Date, @Type)", conn, sqlTransaction);
                                    command.Parameters.AddWithValue("@Acc", transaction.acc_id);
                                    command.Parameters.AddWithValue("@User", transaction.user_id);
                                    command.Parameters.AddWithValue("@Amount", transaction.amount);
                                    command.Parameters.AddWithValue("@Date", transaction.date);
                                    command.Parameters.AddWithValue("@Type", transaction.trans_type);
                                    command.ExecuteNonQuery();

                                    SqlCommand updateCommand = new SqlCommand("UPDATE Accounts SET balance = balance - @Amount WHERE acc_id = @AccID", conn, sqlTransaction);
                                    updateCommand.Parameters.AddWithValue("@Amount", transaction.amount);
                                    updateCommand.Parameters.AddWithValue("@AccID", transaction.acc_id);
                                    updateCommand.ExecuteNonQuery();
                                    message = "Withdrawal transaction added successfully!";
                                }
                                else
                                {
                                    sqlTransaction.Rollback();
                                    message = "Insufficient funds.";
                                }



                            }
                            else
                            {
                                if (currentBalance >= amount)
                                {
                                    SqlCommand command = new SqlCommand("INSERT INTO Transactions (acc_id, user_id, amount, date,trans_type) VALUES (@Acc, @User, @Amount, @Date, @Type)", conn, sqlTransaction);
                                    command.Parameters.AddWithValue("@Acc", transaction.acc_id);
                                    command.Parameters.AddWithValue("@User", transaction.user_id);
                                    command.Parameters.AddWithValue("@Amount", transaction.amount);
                                    command.Parameters.AddWithValue("@Date", transaction.date);
                                    command.Parameters.AddWithValue("@Type", transaction.trans_type);
                                    command.ExecuteNonQuery();

                                    SqlCommand updateCommand = new SqlCommand("UPDATE Accounts SET balance = balance + @Amount WHERE acc_id = @AccID", conn, sqlTransaction);
                                    updateCommand.Parameters.AddWithValue("@Amount", transaction.amount);
                                    updateCommand.Parameters.AddWithValue("@AccID", transaction.acc_id);
                                    updateCommand.ExecuteNonQuery();
                                    message = "Deposit transaction added successfully!";

                                }
                                else
                                {
                                    sqlTransaction.Rollback();
                                    message = "Insufficient funds.";
                                }
                            }
                        
                                    sqlTransaction.Commit();

                        }
                        catch
                        {
                            sqlTransaction.Rollback();
                            throw new Exception("Error occured while adding transaction");
                        }
                    
                }
                   
            }
            return message;
        }

        public string AddExtTransaction(ExtTransactions transaction, decimal amount, int accNum)
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
                            SqlCommand command = new SqlCommand("INSERT INTO ExtTransactions (from_acc_id, from_user_id, to_acc_id, to_user_id, amount, date) VALUES (@FAcc, @FUser, @TAcc, @TUser, @Amount, @Date)", conn, sqlTransaction);
                            command.Parameters.AddWithValue("@FAcc", transaction.from_acc_id);
                            command.Parameters.AddWithValue("@FUser", transaction.from_user_id);
                            command.Parameters.AddWithValue("@TAcc", transaction.to_acc_id);
                            command.Parameters.AddWithValue("@TUser", transaction.to_user_id);
                            command.Parameters.AddWithValue("@Amount", transaction.amount);
                            command.Parameters.AddWithValue("@Date", transaction.date);

                            SqlCommand updateCommand1 = new SqlCommand("UPDATE Accounts SET balance = balance - @Amount WHERE acc_id = @FAccID", conn, sqlTransaction);
                            updateCommand1.Parameters.AddWithValue("@Amount", transaction.amount);
                            updateCommand1.Parameters.AddWithValue("@FAccID", transaction.from_acc_id);

                            SqlCommand updateCommand2 = new SqlCommand("UPDATE Accounts SET balance = balance + @Amount WHERE acc_id = @TAccID", conn, sqlTransaction);
                            updateCommand2.Parameters.AddWithValue("@Amount", transaction.amount);
                            updateCommand2.Parameters.AddWithValue("@TAccID", transaction.to_acc_id);


                            message = "Transaction added successfully!";

                            command.ExecuteNonQuery();
                            updateCommand1.ExecuteNonQuery();
                            updateCommand2.ExecuteNonQuery();

                            sqlTransaction.Commit();

                        }
                        catch
                        {
                            sqlTransaction.Rollback();
                            throw new Exception("Error occured while adding transaction");
                        }
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        message = "Insufficient funds.";
                    }
                }
                conn.Close();
            }
            return message;
        }
        public int GetAccID(int user_id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                SqlCommand command2 = new SqlCommand("SELECT acc_id FROM Accounts WHERE user_id = @User ", conn);
                command2.Parameters.AddWithValue("@User", user_id);
                object result2 = command2.ExecuteScalar();

                if (result2 == null)
                {
                    throw new Exception("User not found.");
                }

                int id = Convert.ToInt32(result2);
                return id;
            }

        }

        public int GetUserID(int acc_id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                SqlCommand command2 = new SqlCommand("SELECT user_id FROM Accounts WHERE acc_id = @Acc ", conn);
                command2.Parameters.AddWithValue("@Acc", acc_id);
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


    public partial class UserTransactions : System.Web.UI.Page
    {
        private void BindTransGrid(string userName, string password)
        {
            UserTransaction transRep = new UserTransaction();
            
            transRep.LoadUserTransactions(gvTransaction, userName, password);
            transRep.LoadExtTransactions(gvExtTrans, userName, password);

        }

        private void BindBalance()
        {
            int acc_id = Convert.ToInt32(Session["AccID"]);
            UserTransaction userTransaction = new UserTransaction();
            List<Accounts> accounts = userTransaction.GetBalance(acc_id);
            gvBalance.DataSource = accounts;
            gvBalance.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userName = Session["Username"]?.ToString();
                string password = Session["Password"]?.ToString();
                if (userName!= null && password != null)
                {
                    lblWelcome.Text = $"Welcome, {userName}!";
                    BindTransGrid(userName, password); 
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }

        }
        

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Transactions newTransaction = new Transactions
            {
                acc_id = Convert.ToInt32(Session["AccID"]),
                user_id = Convert.ToInt32(Session["UserID"]),
                amount = Convert.ToDecimal(txtAmount.Text),
                date = DateTime.Now,
                trans_type = ddlTransactionType.SelectedValue
            };

            UserTransaction userTransaction = new UserTransaction();
           
            lblMessage.Text = userTransaction.AddTransaction(newTransaction, Convert.ToDecimal(txtAmount.Text), Convert.ToInt32(Session["AccID"]));

            gvTransaction.DataBind();
            string userName = Session["Username"]?.ToString();
            string password = Session["Password"]?.ToString();
            BindTransGrid(userName, password);
            BindBalance();
 
            txtAmount.Text = string.Empty;

        }

        protected void btnBalance_Click(object sender, EventArgs e)
        {
            BindBalance();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnTransactions_Click(object sender, EventArgs e)
        {
            Response.Redirect("Transfer.aspx");
        }

        protected void btnEnter2_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            UserTransaction extrans = new UserTransaction();

            ExtTransactions newTrans = new ExtTransactions
            {
                from_acc_id = extrans.GetAccID(userID),
                from_user_id = userID,
                to_acc_id = Convert.ToInt32(txtTo.Text), 
                to_user_id = extrans.GetUserID(Convert.ToInt32(txtTo.Text)), 
                amount = Convert.ToDecimal(txtAmount1.Text),
                date = DateTime.Now
            };


            try
            {
                lblMessage1.Text = extrans.AddExtTransaction(newTrans, Convert.ToDecimal(txtAmount1.Text), extrans.GetAccID(userID));
                BindBalance();
                txtAmount.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage1.Text = "Error: " + ex.Message;
            }
            string userName = Session["Username"]?.ToString();
            string password = Session["Password"]?.ToString();
            BindTransGrid(userName, password);
            BindBalance();
        }
    }
}