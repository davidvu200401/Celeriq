<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="ServerSettings.aspx.cs" Inherits="Celeriq.AdminSite.AuthUser.ServerSettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="maincontainer spacedblock">


<h1>Celeriq Settings</h1>

<div>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorblock" />
</div>


<div class="container">
    <div class="row">
        <div class="col-md-6 col-sm-12 col-xs-12">
            <div class="container">
                <div class="row">
                    <div class="col-xs-4">
                        <span>Max loaded</span>
                    </div>
                    <div class="col-xs-8">
                        <asp:TextBox ID="txtLoaded" runat="server" placeholder="Max loaded" /><span class="explain">0 is unlimited</span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-4">
                        <span>Max memory (MB)</span>
                    </div>
                    <div class="col-xs-8">
                        <asp:TextBox ID="txtMemory" runat="server" placeholder="Max memory (MB)" /><span class="explain">0 is unlimited</span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-4">
                        <span>Unload time (min)</span>
                    </div>
                    <div class="col-xs-8">
                        <asp:TextBox ID="txtTime" runat="server" placeholder="Unload time (min)" /><span class="explain">0 is unlimited</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-sm-12 col-xs-12">
            <div class="infoblock bigboxes">
                <strong><span class="prompt">Repositories total</span></strong><asp:Literal ID="lblRepoTotal" runat="server" /><br />
                <strong><span class="prompt">Repositories in-mem</span></strong><asp:Literal ID="lblLoaded" runat="server" /><br />
                <strong><span class="prompt">Memory used</span></strong><asp:Literal ID="lblCurrentMemory" runat="server" /><br />
                <strong><span class="prompt">Disk size</span></strong><asp:Literal ID="lblDisk" runat="server" /><br />
                <strong><span class="prompt">Machine name</span></strong><asp:Literal ID="lblMachineName" runat="server" /><br />
                <strong><span class="prompt">OS version</span></strong><asp:Literal ID="lblOSVersion" runat="server" /><br />
                <strong><span class="prompt">Processors</span></strong><asp:Literal ID="lblProcessors" runat="server" /><br />
                <strong><span class="prompt">Total memory</span></strong><asp:Literal ID="lblTotalMemory" runat="server" /><br />
                <strong><span class="prompt">Reboot</span></strong><asp:Literal ID="lblReboot" runat="server" /><br />
            </div>
        </div>
    </div>
</div>






<div>
<asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="primary button standard-size" />
</div>


<div>
<hr />

<div style="float:right;" class="celeriqhistoryblock">
View: <asp:Literal ID="lblHistoryHours" runat="server" />
</div>

<h2>History</h2>
<div id="divcpu" class="linegraph"></div>
<div id="divprocessmemory" class="linegraph"></div>
<div id="divmemused" class="linegraph"></div>
<div id="divloadtotal" class="linegraph"></div>
<div id="divrepototal" class="linegraph"></div>
<div id="divloaddelta" class="linegraph"></div>
<div id="divcreatedelta" class="linegraph"></div>
<div id="divdeletedelta" class="linegraph"></div>
</div>

</div>

    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(loadHistory);

        $(document).ready(function () {

            $('.celeriqhistoryblock a[hourindex]').click(function () {
                window.location = '/authuser/serversettings.aspx?hours=' + $(this).attr('hourindex');
                return false;
            });

        });

        function loadHistory() {
            $.ajax({
                type: 'POST',
                url: '/mainservice.asmx/getceleriqhistory',
                data: '{hours:' + $('#__hourindex').val() + '}',
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                success: function (r) {

                    //CPU
                    var v = splitChartArray(r.d[0]);
                    var data = google.visualization.arrayToDataTable(v);
                    var showTextEvery = parseInt((v.length / 10));
                    var options = {
                        title: 'CPU Usage',
                        vAxis: { format: '#', minValue: '0' },
                        hAxis: { minTextSpacing: '50', showTextEvery: showTextEvery },
                        legend: { position: 'none' }
                    };
                    var chart = new google.visualization.LineChart(document.getElementById('divcpu'));
                    chart.draw(data, options);

                    //Celeriq Process Memory
                    v = splitChartArray(r.d[1]);
                    data = google.visualization.arrayToDataTable(v);
                    showTextEvery = parseInt((v.length / 10));
                    options = {
                        title: 'Celeriq Process Memory Used (MB)',
                        vAxis: { format: '#', minValue: '0' },
                        hAxis: { minTextSpacing: '50', showTextEvery: showTextEvery },
                        legend: { position: 'none' }
                    };
                    var chart = new google.visualization.LineChart(document.getElementById('divprocessmemory'));
                    chart.draw(data, options);

                    //Total In Memory
                    v = splitChartArray(r.d[2]);
                    data = google.visualization.arrayToDataTable(v);
                    showTextEvery = parseInt((v.length / 10));
                    options = {
                        title: 'Repositories In Memory',
                        vAxis: { format: '#', minValue: '0' },
                        hAxis: { minTextSpacing: '50', showTextEvery: showTextEvery },
                        legend: { position: 'none' }
                    };
                    chart = new google.visualization.LineChart(document.getElementById('divloadtotal'));
                    chart.draw(data, options);

                    //Total Repositories
                    v = splitChartArray(r.d[3]);
                    data = google.visualization.arrayToDataTable(v);
                    showTextEvery = parseInt((v.length / 10));
                    options = {
                        title: 'Total Repositories',
                        vAxis: { format: '#', minValue: '0' },
                        hAxis: { minTextSpacing: '50', showTextEvery: showTextEvery },
                        legend: { position: 'none' }
                    };
                    chart = new google.visualization.LineChart(document.getElementById('divrepototal'));
                    chart.draw(data, options);

                    //Load Delta
                    v = splitChartArray(r.d[4]);
                    data = google.visualization.arrayToDataTable(v);
                    showTextEvery = parseInt((v.length / 10));
                    options = {
                        title: 'Repositories Load Delta',
                        vAxis: { format: '#', minValue: '0' },
                        hAxis: { minTextSpacing: '50', showTextEvery: showTextEvery },
                        legend: { position: 'none' }
                    };
                    chart = new google.visualization.LineChart(document.getElementById('divloaddelta'));
                    chart.draw(data, options);

                    //Create Delta
                    v = splitChartArray(r.d[5]);
                    data = google.visualization.arrayToDataTable(v);
                    showTextEvery = parseInt((v.length / 10));
                    options = {
                        title: 'Repositories Created Delta',
                        vAxis: { format: '#', minValue: '0' },
                        hAxis: { minTextSpacing: '50', showTextEvery: showTextEvery },
                        legend: { position: 'none' }
                    };
                    chart = new google.visualization.LineChart(document.getElementById('divcreatedelta'));
                    chart.draw(data, options);

                    //Deleted Delta
                    v = splitChartArray(r.d[6]);
                    data = google.visualization.arrayToDataTable(v);
                    showTextEvery = parseInt((v.length / 10));
                    options = {
                        title: 'Repositories Deleted Delta',
                        vAxis: { format: '#', minValue: '0' },
                        hAxis: { minTextSpacing: '50', showTextEvery: showTextEvery },
                        legend: { position: 'none' }
                    };
                    chart = new google.visualization.LineChart(document.getElementById('divdeletedelta'));
                    chart.draw(data, options);

                    //Memory
                    v = splitChartArray(r.d[7]);
                    data = google.visualization.arrayToDataTable(v);
                    showTextEvery = parseInt((v.length / 10));
                    options = {
                        title: 'Memory Used (MB)',
                        vAxis: { format: '#', minValue: '0' },
                        hAxis: { minTextSpacing: '50', showTextEvery: showTextEvery },
                        legend: { position: 'none' }
                    };
                    chart = new google.visualization.LineChart(document.getElementById('divmemused'));
                    chart.draw(data, options);

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert('An error occurred!');
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }

        function splitChartArray(value, a, b) {
            try {
                //23-May-08, 1|24-May-08, 4
                if (value != '') {
                    var arr = value.split('|');
                    var data = [];
                    data.push(['', '']);
                    for (var ii = 0; ii < arr.length; ii++) {
                        var str = arr[ii];
                        var dataArray = [];
                        var array = str.split(',');
                        if (array.length == 2) { //must have 2 elements
                            dataArray.push(array[0].toString());
                            dataArray.push(parseInt(array[1], 10));
                            data.push(dataArray);
                        }
                    }
                    return data;
                } else {
                    return null;
                }
            } catch (e) {
                return null;
            }
        }

    </script>


</asp:Content>
