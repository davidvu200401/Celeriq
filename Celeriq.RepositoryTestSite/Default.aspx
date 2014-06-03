<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="Celeriq.RepositoryTestSite.Default" %>
<%@ Register src="~/UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<uc1:PagingControl ID="PagingControl1" runat="server" />
<asp:GridView ID="grdItem" runat="server" AutoGenerateColumns="false"
CssClass="prettygrid" EnableViewState="false"
HeaderStyle-CssClass="prettygrid-header" RowStyle-CssClass="prettygrid-row" AlternatingRowStyle-CssClass="prettygrid-rowalt"
EmptyDataText="There were no items found to display" EmptyDataRowStyle-CssClass="prettygrid-empty"
AllowSorting="false" Width="100%" ClientIDMode="Static">
<Columns>
<asp:TemplateField HeaderText="Name">
<ItemTemplate>
<asp:Literal ID="lblName" runat="server" EnableViewState="false" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Disk size">
<ItemTemplate>
<asp:Literal ID="lblDisk" runat="server" EnableViewState="false" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Memory size">
<ItemTemplate>
<asp:Literal ID="lblMemory" runat="server" EnableViewState="false" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="IsLoaded" HeaderText="Loaded" />
<asp:TemplateField>
<ItemTemplate>
<asp:Literal ID="lblAction" runat="server" EnableViewState="false" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</asp:Content>
