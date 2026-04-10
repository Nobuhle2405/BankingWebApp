<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transfers.aspx.cs" Inherits="WebApp.Transfers1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
        <div>
            <h1>TRANSFER HISTORY</h1>
                <asp:GridView ID="gvTransfer" runat="server" AutoGenerateColumns="False" DataKeyNames="tf_id" Width="460px" >
                    <Columns>
                        <asp:BoundField DataField="trans_id" HeaderText="Transaction ID" ReadOnly="True" />
                        <asp:BoundField DataField="acc_id" HeaderText="Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="user_id" HeaderText="User ID" ReadOnly="True" />
                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                        <asp:BoundField DataField="date" HeaderText="Date" />
                        <asp:BoundField DataField="trans_type" HeaderText="Type" />
                    </Columns>
                </asp:GridView>
                <br/>
            <h1>BALANCE</h1>
            <asp:Button ID="btnBalance" runat="server" Text="GENERATE BALANCE" OnClick="btnBalance_Click" />
            <asp:GridView ID="gvBalance" runat="server" AutoGenerateColumns="False" DataKeyNames="acc_id" Width="460px" >
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
                <h1>PERFORM TRANSACTION</h1>
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
                <label for="cDate">Date:</label>
                <asp:Calendar ID="cDate" runat="server" />
                <br />
                <br />
                <asp:Button ID="btnEnter" runat="server" Text="ENTER" OnClick="btnEnter_Click" />
                <asp:Label ID="lblMessage" runat="server" ForeColor="DeepPink"></asp:Label>
                <br />
                <br />            
                <asp:Button ID="btnLogout" runat="server" Text="LOG OUT" OnClick="btnLogout_Click" />
                <asp:Button ID="btnTransactions" runat="server" Text="MAKE A TRANSACTION" OnClick="btnTransactions_Click" />
            </div>
    </form>
</body>
</html>

