CREATE DATABASE BankingDB1;
USE BankingDB1;

--TABLES
--User(id,username, password,email..)
--Accounts(id,userid,balance,acctype..)
--Transactions(ID, fromaccid, toaccid, amount,date...)

CREATE TABLE Users(
user_id INT IDENTITY(1,1) PRIMARY KEY ,
username VARCHAR(100) NOT NULL,
password VARCHAR(100) NOT NULL,
email VARCHAR(100) NOT NULL);

CREATE TABLE Accounts(
acc_id INT IDENTITY(101,1) PRIMARY KEY,
user_id INT FOREIGN KEY REFERENCES Users(user_id),
balance FLOAT(10) NOT NULL DEFAULT 0,
acc_type VARCHAR(100) NOT NULL
);

CREATE TABLE Transactions(
trans_id INT IDENTITY(201,1) PRIMARY KEY,
acc_id INT FOREIGN KEY REFERENCES Accounts(acc_id),
user_id INT FOREIGN KEY REFERENCES Users(user_id),
amount FLOAT(10) NOT NULL,
date DATETIME NOT NULL DEFAULT GETDATE(),
trans_type VARCHAR(10)
);

CREATE TABLE Transfers(
tf_id INT IDENTITY(301,1) PRIMARY KEY,
from_acc_id INT FOREIGN KEY REFERENCES Accounts(acc_id),
to_acc_id INT FOREIGN KEY REFERENCES Accounts(acc_id),
user_id INT FOREIGN KEY REFERENCES Users(user_id),
amount FLOAT(10) NOT NULL,
date DATETIME NOT NULL DEFAULT GETDATE()
);
/*ADD INTERNAL TRANSACTION TABLE*/
CREATE TABLE ExtTransactions(
et_id INT IDENTITY(401,1) PRIMARY KEY,
from_acc_id INT FOREIGN KEY REFERENCES Accounts(acc_id),
from_user_id INT FOREIGN KEY REFERENCES Users(user_id),
to_acc_id INT FOREIGN KEY REFERENCES Accounts(acc_id),
to_user_id INT FOREIGN KEY REFERENCES Users(user_id),
amount FLOAT(10) NOT NULL,
date DATETIME NOT NULL DEFAULT GETDATE()
);

drop table Users;
drop table Accounts;
drop table Transactions;

INSERT INTO Transfers(from_acc_id, to_acc_id, user_id, amount, date)
VALUES
(101, 102, 3, 50, '2025-04-20 06:00:00'),
(103, 106, 1, 100, '2025-05-02 12:05:00'),
(104, 105, 2, 150, '2025-05-12 17:30:00')

INSERT INTO Users(username,password,email)
VALUES
('Nobuhle', '123nobu', 'nobuhle@gmail.com'),
('Sarah', 's7R7H', 'sarah@yahoo.com'),
('Sithabile', 'thabi02', 'sithabile@outlook.com'),
('Admin', 'admin123', 'admin@gmail.com')

INSERT INTO Accounts(user_id,balance,acc_type)
VALUES
(3, '10000.00', 'Cheque'),--101
(3, '50000.00', 'Savings'),
(1, '20560.00', 'Savings'),--103
(2, '10000.00', 'Cheque'),
(2, '20000.00', 'Savings'),--105
(1, '50000.00', 'Cheque')

INSERT INTO Transactions(acc_id, user_id, amount, date, trans_type)
VALUES
(106, 1, '500.00', '2025-03-02 12:00:00', 'Withdrawal'),
(101, 3, '1525.00', '2025-03-12 17:30:06','Deposit'),
(106, 1, '6000.00', '2025-03-30 06:12:00','Deposit'),
(104, 2, '2460.00', '2025-03-26 20:26:14', 'Withdrawal')

INSERT INTO ExtTransactions(from_acc_id, from_user_id, to_acc_id, to_user_id, amount, date)
VALUES
(101, 3, 106, 1, '1000', '2025-06-26 20:26:00'),
(104, 2, 101, 3, '1500', '2025-06-27 11:00:14')