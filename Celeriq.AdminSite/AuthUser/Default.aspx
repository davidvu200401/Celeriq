<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="Celeriq.AdminSite.AuthUser.Default" %>
<%@ Register src="~/UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register src="~/UserControls/RepositoryGridItemControl.ascx" tagname="RepositoryGridItemControl" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="maincontainer spacedblock">

    <div>
        <asp:TextBox ID="txtKeyword" runat="server" />
        <asp:Button ID="cmdSearch" runat="server" CssClass="btn btn-flat btn-xs" Text="Search" />
        <asp:Button ID="cmdClear" runat="server" CssClass="btn btn-flat btn-xs" Text="Clear" />
    </div>

    <uc1:PagingControl ID="PagingControl1" runat="server" />
    <asp:Repeater ID="grdList" runat="server" EnableViewState="false">
    <ItemTemplate>
        <uc2:RepositoryGridItemControl ID="RepositoryGridItemControl1" runat="server" />
    </ItemTemplate>
    </asp:Repeater>

</div>


</asp:Content>
