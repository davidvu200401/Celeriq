<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Celeriq.AdminSite.UserControls.PagingControl" Codebehind="PagingControl.ascx.cs" %>
<div style="display:table;width:100%;">
<div style="display:table-row;">
<div style="display: table-cell;padding-left:8px;vertical-align:middle;">
<asp:Literal ID="lblFound" runat="server" EnableViewState="false" />
</div>
<div style="display:table-cell;padding-right:8px;vertical-align:middle;text-align:right;">
<asp:PlaceHolder ID="pnlPagingation" runat="server" EnableViewState="false" />
</div>
</div>
</div>
