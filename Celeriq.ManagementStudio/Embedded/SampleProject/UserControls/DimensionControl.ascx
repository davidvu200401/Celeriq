<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="DimensionControl.ascx.cs" Inherits="CeleriqTestWebsite.UserControls.DimensionControl" EnableViewState="false" %>
<div class="dimensionheader">
<h4><asp:Literal ID="lblHeader" runat="server" /></h4>
</div>
<div class="dimensiondata">
<asp:Repeater ID="rptItem" runat="server">
<ItemTemplate>
<div>
<asp:PlaceHolder ID="pnlItem" runat="server" />
</div>
</ItemTemplate>
</asp:Repeater>
</div>