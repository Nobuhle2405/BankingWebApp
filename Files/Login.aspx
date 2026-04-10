<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApp.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/styles.css" rel="stylesheet" type="text/css" />
        <div>
        <h1>LOGIN:</h1>
        <br />
             <label for="txtUsername">USERNAME:</label><br />
             <asp:TextBox ID="txtUsername" runat="server" />
        <br /><br />
             <label for="txtPassword">PASSWORD:</label><br />
             <asp:TextBox ID="txtPassword" runat="server" />
        <br />
        <br />            
            <asp:Button ID="btnLogin" runat="server" Text="LOGIN" OnClick="btnLogin_Click" CssClass="btn btn-custom" /><br />
            <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Registration.aspx" Text="Don't have a account? REGISTER" OnClick="btnRegister_Click" CssClass="btn btn-link btn-link-custom" />
        <br /> 
            <asp:Label ID="lblMessage" runat="server" ForeColor="DeepPink"></asp:Label>
           </div>
</asp:Content>