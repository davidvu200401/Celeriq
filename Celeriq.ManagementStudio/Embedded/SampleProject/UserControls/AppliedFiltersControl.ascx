<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AppliedFiltersControl.ascx.cs" Inherits="CeleriqTestWebsite.UserControls.AppliedFiltersControl" EnableViewState="false" %>
<div>
<asp:Panel ID="pnlContainer" runat="server">
<div class="dimensioncontainer">
<div class="dimensionheader">
<h4>Applied Job Filters</h4>
</div>
<div class="dimensiondata">
<asp:PlaceHolder ID="pnlFilterDisplay" runat="server" />
<br />
<div align="right" class="leftbarbottomlink">
<asp:PlaceHolder ID="pnlReset" runat="server" />
</div>
</div>
<div style="text-align:right;">
<asp:HyperLink ID="linkClear" runat="server" Text="Clear Filters" />
</div>
</div>
</asp:Panel>
</div>