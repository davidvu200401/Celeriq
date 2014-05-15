<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="true" Inherits="Celeriq.TestingSite.MainSite.UserControls.PagingControl" Codebehind="PagingControl.ascx.cs" %>
<asp:HiddenField ID="rpp" runat="server" Value="0" />
<div class="pagingcontainer">
<table width="100%" class="flattable">
<tr>
<td class="pagingtextsmall" style="width:1%;white-space:nowrap;padding-right:16px;">
<asp:Panel ID="pnlHeader" runat="server">
<asp:Literal ID="lblFound" runat="server" EnableViewState="false" />
</asp:Panel>
</td>
<td class="sortbox" style="display:none;">
<asp:PlaceHolder ID="pnlSort" runat="server"></asp:PlaceHolder>
</td>
<td class="paginggotopage" style="text-align:center;vertical-align:middle;">
<asp:Panel ID="pnlGotoContainer" runat="server" EnableViewState="false">
Go to page: <asp:PlaceHolder ID="pnlGotoPage" runat="server" EnableViewState="false" />
</asp:Panel>
</td>
<td class="paginggotopage" style="width:1%;white-space:nowrap;text-align:right;vertical-align:middle;">
<asp:PlaceHolder ID="pnlPagePer" runat="server" EnableViewState="false">
<asp:DropDownList ID="cboPagePer" runat="server" CssClass="dropdownlist" AutoPostBack="true" EnableViewState="false" />
</asp:PlaceHolder>
</td>
</tr>
</table>
</div>