<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="Login.aspx.cs" Inherits="Celeriq.RepositoryTestSite.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ValidationSummary ID="ValidationSummary1" runat="server" />

<div><asp:TextBox ID="txtServer" runat="server" placeholder="Server" required="true" /></div>
<div><asp:TextBox ID="txtUser" runat="server" placeholder="User" required="true" /></div>
<div><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Password" required="true" /></div>

<div class="buttonblock">
<asp:Button ID="cmdConnect" runat="server" Text="Connect" />
</div>

</asp:Content>
