<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="Celeriq.AdminSite.Default" %>
<%@ Register src="~/UserControls/LoginControl.ascx" tagname="LoginControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div align="center">
    <uc1:LoginControl ID="LoginControl1" runat="server" />
</div>

</asp:Content>
