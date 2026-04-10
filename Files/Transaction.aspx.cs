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
    public class Transactions
    {
        public int trans_id { get; set; }
        public int acc_id { get; set; }
        public int user_id { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public string trans_type { get; set; }
    }

    public class TransactionRepository
    {
        private string connectionString;
        public TransactionRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BankingDB1"].ConnectionString;
        }

        public List<Transactions> GetTransactions()
        {
            List<Transactions> translist = new List<Transactions>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Transactions", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    translist.Add(new Transactions()
                    {
                        trans_id = Convert.ToInt32(reader["trans_id"]),
                        acc_id = Convert.ToInt32(reader["acc_id"]),
                        user_id = Convert.ToInt32(reader["user_id"]),
                        amount = Convert.ToDecimal(reader["amount"]),
                        date = Convert.ToDateTime(reader["date"]),
                        trans_type = reader["trans_type"].ToString()
                    });
                }
            }
            return translist;
        }

        public List<Transactions> GetTransactionsbyAccId(int accID)
        {
            List<Transactions> translist = new List<Transactions>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Transactions WHERE acc_id = @AccID", conn);
                command.Parameters.AddWithValue("@AccID", accID);
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    translist.Add(new Transactions()
                    {
                        trans_id = Convert.ToInt32(reader["trans_id"]),
                        acc_id = Convert.ToInt32(reader["acc_id"]),
                        user_id = Convert.ToInt32(reader["user_id"]),
                        amount = Convert.ToDecimal(reader["amount"]),
                        date = Convert.ToDateTime(reader["date"]),
                        trans_type = reader["trans_type"].ToString()
                    });
                }
            }
            return translist;
        }

        public void AddTransaction(Transactions transaction) //add transaction type to database
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using(var sqlTransaction = conn.BeginTransaction())
                {
                    try 
                    {
                        SqlCommand command = new SqlCommand("INSERT INTO Transactions (acc_id, user_id, amount, date,trans_type) VALUES (@AccID, @UserID, @Amount, @Date, @Type)", conn, sqlTransaction);
                        command.Parameters.AddWithValue("@AccID", transaction.acc_id);
                        command.Parameters.AddWithValue("@UserID", transaction.user_id);
                        command.Parameters.AddWithValue("@Amount", transaction.amount);
                        command.Parameters.AddWithValue("@Date", transaction.date);
                        command.Parameters.AddWithValue("@Type", transaction.trans_type);
                        command.ExecuteNonQuery();

                        SqlCommand updateCommand = new SqlCommand("UPDATE Accounts SET balance = balance + @Amount WHERE acc_id = @AccID", conn, sqlTransaction);
                        if (transaction.trans_type == "Withdrawal") 
                        {
                            updateCommand.CommandText = "UPDATE Accounts SET balance = balance - @Amount WHERE acc_id = @AccId";
                        }
                        updateCommand.Parameters.AddWithValue("@Amount", transaction.amount);
                        updateCommand.Parameters.AddWithValue("@AccID", transaction.acc_id);
                        command.ExecuteNonQuery();

                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }

        }

        public void DeleteTransaction(Transactions transaction)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("DELETE FROM Transactions WHERE trans_id = @trans_id", connection);
                command.Parameters.AddWithValue("@trans_id", transaction.trans_id);
                command.ExecuteNonQuery();
            
            }
        }

    }

    public partial class Transaction : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BankingDB1"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTransactionGrid();
            }
        }

        //Add methods for performing transactions

        private void BindTransactionGrid()
        {
            TransactionRepository transRep = new TransactionRepository();
            List<Transactions> transList = transRep.GetTransactions();
            gvTransaction1.DataSource = transList;
            gvTransaction1.DataBind();
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            int accID = Convert.ToInt32(txtAccID.Text);

            TransactionRepository transRep = new TransactionRepository();
            List<Transactions> transList = transRep.GetTransactionsbyAccId(accID);
            gvTransaction.DataSource = transList;
            gvTransaction.DataBind();
            
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Transactions deleteTrans = new Transactions
            {
                trans_id = Convert.ToInt32(txtID4.Text)
            };

            TransactionRepository transRep = new TransactionRepository();
            transRep.DeleteTransaction(deleteTrans);

            txtID4.Text = string.Empty;

            BindTransactionGrid();
        }
    }
}