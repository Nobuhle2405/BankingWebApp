<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="WebApp.Customer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>

    <h2>WELCOME TO THE CUSTOMER PAGE!!</h2>
    <br /><br />
    <formview>
        <div>
            <h1>CUSTOMER LIST</h1>
            <div class="table-responsive">
                 <asp:GridView ID="gvCustomer" runat="server" CssClass="table table-striped table-bordered table-hover table-custom" AutoGenerateColumns="false" DataKeyNames="user_id">
                     <Columns>
                            <asp:BoundField DataField="user_id" HeaderText="ID" ReadOnly="True" />
                            <asp:BoundField DataField="username" HeaderText="Name" />
                            <asp:BoundField DataField="email" HeaderText="Email" />
                            <asp:BoundField DataField="password" HeaderText="Password" />
                     </Columns>
                 </asp:GridView> 
            </div>
       </div>
    </formview>
    <br /><br />
    <formview>
        <div>
            <h1>ADD NEW CUSTOMERS</h1>
                   <label for="txtName">Username:</label><br />
                   <asp:TextBox ID="txtName" runat="server" />
               <br /><br />
                   <label for="txtEmail">Email:</label><br />
                   <asp:TextBox ID="txtEmail" runat="server" />
               <br /><br />
                   <label for="txtPassword">Password:</label><br />
                   <asp:TextBox ID="txtPassword" runat="server" />
               <br /><br />
                   <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" OnClick="btnSubmit_Click" CssClass="btn btn-custom" /><br />
                    <asp:Label ID="lblMessage" runat="server" ForeColor="DeepPink" ></asp:Label>
             </div>
    </formview>  
    <br /><br />
    <formview>
        <div>
            <h1>UPDATE CUSTOMER DETAILS</h1>
                   <label for="txtID">User ID:</label><br />
                   <asp:TextBox ID="txtID" runat="server" />
               <br /><br />
                   <label for="txtName">Username:</label><br />
                   <asp:TextBox ID="txtName1" runat="server" />
               <br /><br />
                   <label for="txtEmail">Email:</label><br />
                   <asp:TextBox ID="txtEmail1" runat="server" />
               <br /><br />
                   <label for="txtPassword">Password:</label><br />
                   <asp:TextBox ID="txtPassword1" runat="server" />
               <br /><br />
                   <asp:Button ID="btnUpdate" runat="server" Text="UPDATE" OnClick="btnUpdate_Click" CssClass="btn btn-custom" />
         </div>  
    </formview>
    <br /><br />
    <formview>
        <div>
        <h1>DELETE CUSTOMERS</h1>
               <label for="txtID">User ID:</label><br />
               <asp:TextBox ID="txtID2" runat="server" />
           <br /><br />
               <asp:Button ID="btnDELETE" runat="server" Text="DELETE" OnClick="btnDelete_Click" CssClass="btn btn-custom" />
         </div>            
    </formview>
</asp:Content>