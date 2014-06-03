<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="Results.aspx.cs" Inherits="Celeriq.RepositoryTestSite.Results" %>
<%@ Register src="~/UserControls/DimensionListControl.ascx" tagname="DimensionListControl" tagprefix="uc1" %>
<%@ Register src="~/UserControls/ListingCell.ascx" tagname="ListingCell" tagprefix="uc2" %>

<%@ Register src="UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc3" %>

<%@ Register src="UserControls/AppliedFilterControl.ascx" tagname="AppliedFilterControl" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<table style="width:100%;">
<tr>
<td style="width:250px;">
<uc4:AppliedFilterControl ID="AppliedFilterControl1" runat="server" />
<uc1:DimensionListControl ID="DimensionListControl1" runat="server" />
</td>
<td>

<uc3:PagingControl ID="PagingControl1" runat="server" />
<asp:DataList ID="grdList" runat="server" EnableViewState="false" Width="100%">
<ItemTemplate>
<uc2:ListingCell ID="ListingCell1" runat="server" />
</ItemTemplate>
</asp:DataList>

</td>
</tr>
</table>

</asp:Content>
