<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="CeleriqTestWebsite.Default" %>
<%@ Register src="~/UserControls/DimensionControl.ascx" tagname="DimensionControl" tagprefix="uc1" %>
<%@ Register src="~/UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>
<%@ Register src="~/UserControls/AppliedFiltersControl.ascx" tagname="AppliedFiltersControl" tagprefix="uc3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" class="main-container">
        <div class="main-block">
            <table style="width:100%;">
                <tr>
                    <td style="width: 200px;">
                        <div class="dimension-block">
                        <uc3:AppliedFiltersControl ID="AppliedFiltersControl1" runat="server" EnableViewState="false" />
                        <asp:Repeater ID="rptDimension" runat="server" EnableViewState="false">
                            <ItemTemplate>
                                <uc1:DimensionControl ID="DimensionControl1" runat="server" EnableViewState="false" />
                            </ItemTemplate>
                        </asp:Repeater>
                        </div>
                    </td>
                    <td>
                        <uc2:PagingControl ID="PagingControl1" runat="server" />
                        <asp:GridView ID="grdResults" runat="server" AutoGenerateColumns="true" CssClass="prettygrid" EnableViewState="false"
                            HeaderStyle-CssClass="prettygrid-header" RowStyle-CssClass="prettygrid-row" AlternatingRowStyle-CssClass="prettygrid-rowalt">
                        </asp:GridView>
                    </td>	
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
