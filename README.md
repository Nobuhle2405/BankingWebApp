# Nobuhle's Banking Web App

A web-based banking application built with **ASP.NET Web Forms (C#)** and **SQL Server**. The app allows users to register, log in, manage accounts, perform transactions and transfers between accounts.

---

## Tech Stack

- **Frontend:** ASP.NET Web Forms, Bootstrap 5, HTML/CSS
- **Backend:** C# (.NET Framework 4.7.2)
- **Database:** Microsoft SQL Server (T-SQL)
- **IDE:** Visual Studio 2022

---

## Features

### Admin
- View all registered customers
- Add, update and delete customers
- View all accounts and transactions

### Customer
- Register and log in securely
- View personal transaction history
- Perform deposits and withdrawals (Internal Transactions)
- Transfer funds to another account (External Transactions)
- Transfer funds between own accounts (Cheque ↔ Savings)
- View current account balance

---

## Project Structure

```
WebApp/
│
├── Login.aspx / .cs          # Login page
├── Registration.aspx / .cs   # New user registration
├── Customer.aspx / .cs       # Admin - manage customers
├── Account.aspx / .cs        # Admin - manage accounts
├── Transaction.aspx / .cs    # Admin - view/delete transactions
├── UserTransactions.aspx / .cs  # Customer - internal & external transactions
├── Transfer.aspx / .cs       # Customer - transfers between own accounts
├── Site.Master / .cs         # Shared master page with navigation
├── BankingAppDB.sql           # Database schema and seed data
└── Content/styles.css        # Custom styles
```

---

## Database Schema

| Table | Description |
|---|---|
| `Users` | Stores customer login credentials |
| `Accounts` | Stores account details linked to a user |
| `Transactions` | Records deposits and withdrawals |
| `Transfers` | Records transfers between a user's own accounts |
| `ExtTransactions` | Records transfers between different users |

---

## Getting Started

### Prerequisites
- Visual Studio 2022
- SQL Server (or SQL Server Express)
- .NET Framework 4.7.2

### Setup Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/your-repo-name.git
   ```

2. **Set up the database**
   - Open SQL Server Management Studio (SSMS)
   - Run the `BankingAppDB.sql` file to create the database, tables and seed data

3. **Configure the connection string**
   - Open `Web.config`
   - Update the `BankingDB1` connection string to match your SQL Server instance:
   ```xml
   <connectionStrings>
     <add name="BankingDB1"
          connectionString="Data Source=YOUR_SERVER;Initial Catalog=BankingDB1;Integrated Security=True"
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **Run the application**
   - Open `WebApp.sln` in Visual Studio
   - Press **F5** or click **Start** to run with IIS Express

---

## Default Login Credentials

| Role | Username | Password |
|---|---|---|
| Admin | Admin | admin123 |
| Customer | Nobuhle | 123nobu |
| Customer | Sarah | s7R7H |
| Customer | Sithabile | thabi02 |

> **Note:** Passwords are currently stored in plain text. Hashing is recommended before deploying to production.

---

## Known Limitations

- Passwords are not hashed — for academic/demo purposes only
- No email verification on registration
- Session-based authentication (no JWT or OAuth)

---

## Author

**Nobuhle**  
Built as a student project using ASP.NET Web Forms and SQL Server.
