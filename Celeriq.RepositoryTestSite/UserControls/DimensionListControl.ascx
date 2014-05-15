<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="DimensionListControl.ascx.cs" Inherits="Celeriq.RepositoryTestSite.UserControls.DimensionListControl" %>

<div class="results-action-block" style="height: auto;">
<a href="#" class="expand-all" title="Collapse all sections">expand all</a><span style="padding-left: 30px;">&nbsp;</span>
<a href="#" class="collapse-all" title="Collapse all sections">collapse all</a>
</div>

<div class="dimensionblock">
<asp:DataList ID="lstItem" runat="server" Width="100%" EnableViewState="false">
<ItemTemplate>
<div class="dimensioncontainer">
<div class="dimensionheader">
<h4><asp:PlaceHolder ID="pnlHeader" runat="server" /></h4>
</div>
<asp:Panel ID="pnlInside" runat="server" CssClass="dimensiondata" />
</div>
</ItemTemplate>
</asp:DataList>
<asp:Panel ID="pnlNoResults" runat="server" Visible="false">
</asp:Panel>
</div>

<div class="explain" style="margin-top:16px;">
Note: Filters with only one or no items will not be shown in the list above.
</div>
