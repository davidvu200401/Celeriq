<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="LoginControl.ascx.cs" Inherits="Celeriq.AdminSite.UserControls.LoginControl" %>

<div class="login-box">

<h1>Site Login</h1>

<fieldset class="col-sm-12">

<div class="form-group">
<label class="control-label" for="username">Server</label>
<div class="controls row">
<div class="input-group col-sm-12">
<input type="text" class="form-control" id="servername" value="localhost" />
</div>
</div>
</div>

<div class="form-group">
<label class="control-label" for="username">Username</label>
<div class="controls row">
<div class="input-group col-sm-12">
<input type="text" class="form-control" id="username" value="root" />
</div>
</div>
</div>

<div class="form-group">
<label class="control-label" for="password">Password</label>
<div class="controls row">
<div class="input-group col-sm-12">
<input type="password" class="form-control" id="password" value="password" />
</div>	
</div>
</div>

<div class="row">
<button type="button" id="cmdLogin" class="btn btn-flat col-xs-12">Login</button>
</div>

</fieldset>

</div>

<script>
    $(document).ready(function () {

        $('#cmdLogin').click(function () {
            $(this).attr('disabled', 'disabled');

            //$.ajax({
            //    type: "POST",
            //    url: "/mainservice.asmx/logout",
            //    contentType: "application/json",
            //    dataType: "json",
            //    success: function (r) {
            //        $('#cmdLogin').removeAttr('disabled');
            //    },
            //    error: function () {
            //        alert('The error has occurred.');
            //    }
            //});


            $.ajax({
                type: "POST",
                url: "/mainservice.asmx/login",
                data: '{servername: "' + $('#servername').val() + '", username: "' + $('#username').val() + '", password: "' + $('#password').val() + '"}',
                contentType: "application/json",
                dataType: "json",
                success: function (r) {
                    $('#cmdLogin').removeAttr('disabled');
                    if (r.d)
                    {
                        window.location.reload();
                    }
                    else
                    {
                        alert('The login as not successful.');
                    }
                },
                error:function()
                {
                    $('#cmdLogin').removeAttr('disabled');
                    alert('The error has occurred.');
                }
            });
            return false;
        });

    });
</script>