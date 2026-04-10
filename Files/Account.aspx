<%@ Page Title="Accounts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="WebApp.Account" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />    
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    
        
        <h2>WELCOME TO THE ACCOUNT PAGE!!</h2>
    <div>
        <br /><br />
    <formview>
        
        <h1>ALL CUSTOMER ACCOUNTS</h1>
            <div class="table-responsive">
                <asp:GridView ID="gvAccount" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="false" DataKeyNames="acc_id" >
                    <Columns>
                        <asp:BoundField DataField="acc_id" HeaderText="Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="user_id" HeaderText="User ID" ReadOnly="True" />
                        <asp:BoundField DataField="balance" HeaderText="Balance" />
                        <asp:BoundField DataField="acc_type" HeaderText="Type" />
                    </Columns>
                </asp:GridView> 
            </div>
        
    </formview>
    </div>
    <br />
    <br />
    <formview>
        <div>
        <h1>SPECIFIC CUSTOMER ACCOUNTS</h1>
            <label for="txtID">User ID:</label><br />
            <asp:TextBox ID="txtID" runat="server" />
        <br /><br />
            <asp:Button ID="btnEnter" runat="server" Text="ENTER" OnClick="btnEnter_Click" CssClass="btn btn-custom" />
        <br /><br /> 
            <div class="table-responsive">
               <asp:GridView ID="gvAccount1" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="false" DataKeyNames="acc_id">
                    <Columns>
                        <asp:BoundField DataField="acc_id" HeaderText="Account ID" ReadOnly="True" />
                        <asp:BoundField DataField="user_id" HeaderText="User ID" ReadOnly="True" />
                        <asp:BoundField DataField="balance" HeaderText="Balance" />
                        <asp:BoundField DataField="acc_type" HeaderText="Type" />
                    </Columns>
                </asp:GridView> 
            </div>
        </div>
    </formview>
    <br />
    <br />
    <formview>
        <div>
        <h1>ADD NEW ACCOUNTS</h1>
               <label for="txtID2">User ID:</label><br />
               <asp:TextBox ID="txtID2" runat="server" />
           <br /><br />
               <label for="txtType">Account Type:</label><br />
               <asp:TextBox ID="txtType" runat="server" />
           <br /><br />
               <asp:Button ID="btnAdd" runat="server" Text="ADD" OnClick="btnAdd_Click" CssClass="btn btn-custom" />
         </div>            
    </formview>
    <br />
    <br />
    <formview>
        <div>
        <h1>UPDATE ACCOUNT DETAILS</h1>
               <label for="txtAccID">Account ID:</label><br />
               <asp:TextBox ID="txtAccID2" runat="server" />
           <br /><br />
               <label for="txtType2">Account Type:</label><br />
               <asp:TextBox ID="txtType2" runat="server" />
           <br /><br />
               <asp:Button ID="btnUpdate" runat="server" Text="UPDATE" OnClick="btnUpdate_Click" CssClass="btn btn-custom" />
         </div>            
    </formview>
    <br />
    <br />
    <formview>
        <div>
        <h1>DELETE ACCOUNTS</h1>
               <label for="txtID4">Account ID:</label><br />
               <asp:TextBox ID="txtID4" runat="server" />
           <br /><br />
               <asp:Button ID="btnDELETE" runat="server" Text="DELETE" OnClick="btnDelete_Click" CssClass="btn btn-custom" />
        </div>            
    </formview>

</asp:Content>
