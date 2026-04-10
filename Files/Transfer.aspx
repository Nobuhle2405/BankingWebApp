<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="WebApp.Transfer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>TRANSFER HISTORY</h1>
                <asp:GridView ID="gvTransfer" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="False" DataKeyNames="tf_id" Width="460px" >
                    <Columns>
                        <asp:BoundField DataField="tf_id" HeaderText="Transfer ID" ReadOnly="True" />
                        <asp:BoundField DataField="from_acc_id" HeaderText="Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="to_acc_id" HeaderText="Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="user_id" HeaderText="User ID" ReadOnly="True" />
                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                        <asp:BoundField DataField="date" HeaderText="Date" />
                    </Columns>
                </asp:GridView>
                <br/>
            <h1>BALANCE</h1>
            <asp:Button ID="btnBalance" runat="server" Text="GENERATE BALANCE" OnClick="btnBalance_Click" CssClass="btn btn-custom" />
            <asp:GridView ID="gvBalance" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="False" DataKeyNames="acc_id" Width="460px" >
                    <Columns>
                        <asp:BoundField DataField="balance" HeaderText="Balance" ReadOnly="True" />
                        <asp:BoundField DataField="acc_type" HeaderText="Account Type" ReadOnly="True" />
                    </Columns>
                </asp:GridView>
                <br />
                <br />
        </div>
        <br /><br />
        <div>
                <h1>PERFORM TRANSFER</h1>
                <label for="ddlFAccType">From Account:</label>
                <asp:DropDownList ID="ddlFAccType" runat="server">
                    <asp:ListItem Text="Cheque" Value="Cheque"></asp:ListItem>
                    <asp:ListItem Text="Savings" Value="Savings"></asp:ListItem> 
                </asp:DropDownList>
            <br /><br />
                <label for="ddlTAccType">To Account:</label>
                <asp:DropDownList ID="ddlTAccType" runat="server">
                    <asp:ListItem Text="Cheque" Value="Cheque"></asp:ListItem>
                    <asp:ListItem Text="Savings" Value="Savings"></asp:ListItem> 
                </asp:DropDownList>
            <br /><br />
                <label for="txtAmount">Amount:</label>
                <asp:TextBox ID="txtAmount" runat="server" />
                <br />
                <br />
                <asp:Button ID="btnEnter" runat="server" Text="ENTER" OnClick="btnEnter_Click" CssClass="btn btn-custom" />
                <asp:Label ID="lblMessage" runat="server" ForeColor="DeepPink"></asp:Label>
                <br />
                <br />            
                <asp:Button ID="btnLogout" runat="server" Text="LOG OUT" OnClick="btnLogout_Click" CssClass="btn btn-custom" />
                <asp:Button ID="btnTransactions" runat="server" Text="MAKE A TRANSACTION" OnClick="btnTransactions_Click" CssClass="btn btn-custom" />
            </div>
    </form>
</body>
</html>
