<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="WebApp.Registration" %>

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
        <h1>REGISTRATION</h1>
               <label for="txtName">Username:</label><br />
               <asp:TextBox ID="txtName" runat="server" />
            <br /><br />
               <label for="txtEmail">Email:</label><br />
               <asp:TextBox ID="txtEmail" runat="server" />
            <br /><br />
               <label for="txtPassword">Password:</label><br />
               <asp:TextBox ID="txtPassword" runat="server" />
            <br /><br />
               <asp:Button ID="btnSubmit" runat="server" Text="REGISTER" OnClick="btnSubmit_Click" CssClass="btn btn-custom" />
            <br />
           <asp:Label ID="lblMessage" runat="server" ForeColor="DeepPink"></asp:Label>
            <br />
            <asp:Button ID="btnLogout" runat="server" Text="GO BACK TO LOGIN" OnClick="btnLogout_Click" CssClass="btn btn-custom" />
        </div>
    </form>
</body>
</html>