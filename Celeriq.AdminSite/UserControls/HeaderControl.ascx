<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="HeaderControl.ascx.cs" Inherits="Celeriq.AdminSite.UserControls.HeaderControl" %>

<div class="header">
    <div class="maincontainer">

        <span class="logo">
            <a href="/">
                <i class="fa fa-list"></i>Celeriq Admin
            </a>
        </span>

        <div style="float:right;margin-top:20px;">
            <asp:PlaceHolder ID="pnlLoggedIn" runat="server">
                <a href="/authuser/">Repositories</a>
                <a href="/authuser/serversettings.aspx">Settings</a>
                <a href="#" id="linkLogout">Logout</a>
            </asp:PlaceHolder>
        </div>

    </div>
</div>