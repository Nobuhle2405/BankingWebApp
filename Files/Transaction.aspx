<%@ Page Title="Transactions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transaction.aspx.cs" Inherits="WebApp.Transaction" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <h2>WELCOME TO THE TRANSACTION PAGE!!</h2>
    <br /><br />
    
    <formview>
        <div>
        <h1>TRANSACTION DETAILS</h1>
           <div class="table-responsive">
                <asp:GridView ID="gvTransaction1" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="false" DataKeyNames="trans_id" >
                    <Columns>
                        <asp:BoundField DataField="trans_id" HeaderText="Transaction ID" ReadOnly="True" />
                        <asp:BoundField DataField="acc_id" HeaderText="Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                        <asp:BoundField DataField="date" HeaderText="Date" />
                        <asp:BoundField DataField="trans_type" HeaderText="Type" />
                    </Columns>
                </asp:GridView> 
           </div>
        </div>
    </formview> 
    <formview>
    <br />
    <br />
        
    <div>
        <h1>ACCOUNT TRANSACTIONS</h1>
        <label for="txtAccID">Account ID:</label><br />
            <asp:TextBox ID="txtAccID" runat="server" />
        <br /><br />
            <asp:Button ID="btnEnter" runat="server" Text="ENTER" OnClick="btnEnter_Click" CssClass="btn btn-custom" />
            <div class="table-responsive">
                <asp:GridView ID="gvTransaction" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="false" DataKeyNames="trans_id" >
                    <Columns>
                        <asp:BoundField DataField="trans_id" HeaderText="Transaction ID" ReadOnly="True" />
                        <asp:BoundField DataField="acc_id" HeaderText="Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                        <asp:BoundField DataField="date" HeaderText="Date" />
                        <asp:BoundField DataField="trans_type" HeaderText="Type" />
                    </Columns>
                </asp:GridView> 
            </div>
    </div>
    </formview>
    <br />
    <br />
    <formview>
        <div>
        <h1>DELETE TRANSACTIONS</h1>
               <label for="txtID4">Transaction ID:</label><br />
               <asp:TextBox ID="txtID4" runat="server" />
           <br /><br />
               <asp:Button ID="btnDELETE" runat="server" Text="DELETE" OnClick="btnDelete_Click" CssClass="btn btn-custom" />
        </div>            
    </formview>
    
</asp:Content>
