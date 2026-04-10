using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApp
{
    public class Accounts
    {
        public int acc_id { get; set; }
        public int user_id { get; set; }
        public decimal balance { get; set; }
        public string acc_type { get; set; }
    }
    public class AccountRepository
    {

        private string connectionString;
        public AccountRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BankingDB1"].ConnectionString;
        }

        public List<Accounts> GetAccounts()
        {
            List<Accounts> acclist = new List<Accounts>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Accounts", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    acclist.Add(new Accounts()
                    {
                        acc_id = Convert.ToInt32(reader["acc_id"]),
                        user_id = Convert.ToInt32(reader["user_id"]),
                        balance = Convert.ToDecimal(reader["balance"]),
                        acc_type = reader["acc_type"].ToString()
                    });
                }
            }
            return acclist;
        }

        public List<Accounts> GetAccountsbyUserId(int userID)
        {
            List<Accounts> acclist = new List<Accounts>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Accounts WHERE user_id = @UserID", conn);
                command.Parameters.AddWithValue("@UserID", userID);
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    acclist.Add(new Accounts()
                    {
                        acc_id = Convert.ToInt32(reader["acc_id"]),
                        user_id = Convert.ToInt32(reader["user_id"]),
                        balance = Convert.ToDecimal(reader["balance"]),
                        acc_type = reader["acc_type"].ToString()
                    });
                }
            }
            return acclist;
        }

        public void AddAccounts(Accounts account)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Accounts (user_id, balance, acc_type) VALUES (@UserID, @Balance, @AccT)", conn);
                command.Parameters.AddWithValue("@UserID", account.user_id);
                command.Parameters.AddWithValue("@Balance", account.balance);
                command.Parameters.AddWithValue("@AccT", account.acc_type);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateAccounts(Accounts account)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("UPDATE Accounts SET  acc_type = @Acc_type WHERE acc_id = @AccID", conn);
                command.Parameters.AddWithValue("@AccID", account.acc_id);
                command.Parameters.AddWithValue("@Acc_type", account.acc_type);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAccounts(Accounts account)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    
                    SqlCommand command = new SqlCommand("DELETE FROM Transactions WHERE acc_id = @acc_id", connection, transaction);
                    command.Parameters.AddWithValue("@acc_id", account.acc_id);
                    command.ExecuteNonQuery();

                    
                    SqlCommand command2 = new SqlCommand( "DELETE FROM Accounts WHERE acc_id = @acc_id", connection, transaction);
                    command2.Parameters.AddWithValue("@acc_id", account.acc_id);
                    command2.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error deleting account: " + ex.Message);
                }
            }
        }

    }
    public partial class Account : Page
    {
        private void BindAccountGrid()
        {
            AccountRepository accRep = new AccountRepository();
            List<Accounts> accList = accRep.GetAccounts();
            gvAccount.DataSource = accList;
            gvAccount.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAccountGrid();
            }
        }
        

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(txtID.Text);
           // BindAccountGrid2(userID);
                AccountRepository accRep = new AccountRepository();
                List<Accounts> accList = accRep.GetAccountsbyUserId(userID);
                gvAccount1.DataSource = accList;
                gvAccount1.DataBind();
                txtID.Text = string.Empty;
            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Accounts newAccount = new Accounts
            {
                user_id = Convert.ToInt32(txtID2.Text),
                acc_type = txtType.Text
            };

            AccountRepository accRep = new AccountRepository();
            accRep.AddAccounts(newAccount);

            txtType.Text = string.Empty;
            txtID2.Text = string.Empty;

            BindAccountGrid();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
           
            Accounts newAccount = new Accounts
            {
                acc_id = Convert.ToInt32(txtAccID2.Text),
                acc_type = txtType2.Text
            };

            AccountRepository accRep = new AccountRepository();
            accRep.UpdateAccounts(newAccount);

            txtAccID2.Text = string.Empty;
            txtType2.Text = string.Empty;

            BindAccountGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Accounts newAccount = new Accounts
            {
                acc_id = Convert.ToInt32(txtID4.Text)
            };

            AccountRepository accRep = new AccountRepository();
            accRep.DeleteAccounts(newAccount);

            txtID4.Text = string.Empty;

            BindAccountGrid();
        }
    }
}