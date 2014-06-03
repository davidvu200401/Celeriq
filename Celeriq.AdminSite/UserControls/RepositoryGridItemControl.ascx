<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RepositoryGridItemControl.ascx.cs" Inherits="Celeriq.AdminSite.UserControls.RepositoryGridItemControl" %>

<div class="repo-griditem <%= this.ClassName %>">

    <asp:Literal ID="lblID" runat="server" />
    <asp:Literal ID="lblName" runat="server" />
    <asp:Literal ID="lblDisk" runat="server" />
    <asp:Literal ID="lblMemory" runat="server" />
    <asp:Literal ID="lblHash" runat="server" />
    <asp:Literal ID="lblCount" runat="server" />
    <asp:Literal ID="lblCreated" runat="server" />

    <div>
        <asp:Literal ID="lblAction" runat="server" />
    </div>

</div>