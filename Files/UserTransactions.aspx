<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserTransactions.aspx.cs" Inherits="WebApp.UserTransactions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
</head>
<body>
    <form id="form1" runat="server">
                <asp:Label ID="lblWelcome" runat="server" Text=""></asp:Label>
                <br />
        <h1>PERFORM INTERNAL TRANSACTIONS:</h1>
        <br />
        <div>
                <h1>INTERNAL TRANSACTION HISTORY</h1>
            <div class="table-responsive">
               <asp:GridView ID="gvTransaction" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="False" DataKeyNames="trans_id" >
                    <Columns>
                        <asp:BoundField DataField="trans_id" HeaderText="Transaction ID" ReadOnly="True" />
                        <asp:BoundField DataField="acc_id" HeaderText="Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                        <asp:BoundField DataField="date" HeaderText="Date" />
                        <asp:BoundField DataField="trans_type" HeaderText="Type" />
                    </Columns>
                </asp:GridView>
             </div>
                <br/>
            
                <br/>
            <h1>BALANCE</h1>
            <asp:Button ID="btnBalance" runat="server" Text="GENERATE BALANCE" OnClick="btnBalance_Click" CssClass="btn btn-custom" />
            <asp:GridView ID="gvBalance" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="False" DataKeyNames="acc_id" >
                    <Columns>
                        <asp:BoundField DataField="balance" HeaderText="Balance" ReadOnly="True" />
                    </Columns>
                </asp:GridView>
                <br />
                <br />
        </div>
        <br /><br />
        <div>
                <h1>PERFORM TRANSACTION</h1>
                <label for="txtAmount">Amount:</label>
                <asp:TextBox ID="txtAmount" runat="server" />
                <br />
                <br />
                <label for="ddlTransactionType">Transaction Type:</label>
                <asp:DropDownList ID="ddlTransactionType" runat="server">
                    <asp:ListItem Text="Deposit" Value="Deposit"></asp:ListItem>
                    <asp:ListItem Text="Withdrawal" Value="Withdrawal"></asp:ListItem> 
                </asp:DropDownList>
                <br />
                <br />
                <asp:Button ID="btnEnter" runat="server" Text="ENTER" OnClick="btnEnter_Click" CssClass="btn btn-custom" />
                <asp:Label ID="lblMessage" runat="server" ForeColor="DeepPink"></asp:Label>
                <br />
            </div>
    
    <h1>PERFORM EXTERNAL TRANSACTIONS:</h1>
        
        <br />
        
        <h1>EXTERNAL TRANSACTION HISTORY</h1>
        <div class="table-responsive">
                <asp:GridView ID="gvExtTrans" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="False" DataKeyNames="et_id"  >
                    <Columns>
                        <asp:BoundField DataField="et_id" HeaderText="Transaction ID" ReadOnly="True" />
                        <asp:BoundField DataField="from_acc_id" HeaderText="From Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="to_acc_id" HeaderText="To Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                        <asp:BoundField DataField="date" HeaderText="Date" />
                    </Columns>
                </asp:GridView>
           </div>
                <br/>
        <div>
                <h1>PERFORM TRANSACTION</h1>
                <label for="txtTAccount">To Account:</label>
                <asp:TextBox ID="txtTo" runat="server" />
                <br /><br />
                <label for="txtAmount">Amount:</label>
                <asp:TextBox ID="txtAmount1" runat="server" />
                <br />
                <br />
                <asp:Button ID="btnEnter2" runat="server" Text="ENTER" OnClick="btnEnter2_Click" CssClass="btn btn-custom" />
                <asp:Label ID="lblMessage1" runat="server" ForeColor="DeepPink"></asp:Label>
                <br />
                <br />            
                <asp:Button ID="btnLogout" runat="server" Text="LOG OUT" OnClick="btnLogout_Click" CssClass="btn btn-custom" />
                <asp:Button ID="btnTransactions" runat="server" Text="MAKE A TRANSFER" OnClick="btnTransactions_Click" CssClass="btn btn-custom" />
            </div>
    </form>
</body>
</html>
