<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AppliedFilterControl.ascx.cs" Inherits="Celeriq.RepositoryTestSite.UserControls.AppliedFilterControl" %>
<div class="resultsfilter">
<asp:Literal ID="lblClear" runat="server" EnableViewState="false" />
<strong>Applied Filters: </strong>
<asp:PlaceHolder ID="pnlFilterDisplay" runat="server" />
</div>
