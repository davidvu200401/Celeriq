$(document).ready(function () {

    $('#linkLogout').click(function () {

            $.ajax({
                type: "POST",
                url: "/mainservice.asmx/logout",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    window.location.reload();
                }
            });

        return false;
    });

});