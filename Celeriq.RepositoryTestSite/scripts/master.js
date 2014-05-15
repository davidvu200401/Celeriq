$(document).ready(function() {

    //Toggle slide the dimension header
    $('.dimensionheader h4').click(function(e) {
        var icon = $(this).children('img:first-child');
        if (icon.attr('expanded') == '1') {
            //Is expanded
            icon.attr('src', '/images/dim-left.png');
            icon.attr('expanded', '0');
            icon.closest('.dimensionheader').next().slideUp(90);
        } else {
            //Is collapsed
            icon.attr('src', '/images/dim-down.png');
            icon.attr('expanded', '1');
            icon.closest('.dimensionheader').next().slideDown(90);
        }
        e.cancelBubble = true;
        return false;
    });

    $('.expand-all').click(function () {
        $('.dimensionheader h4 .toggle').attr('expanded', '0');
        $('.dimensionheader h4').click();
        return false;
    });

    $('.collapse-all').click(function () {
        $('.dimensionheader h4 .toggle').attr('expanded', '1');
        $('.dimensionheader h4').click();
        return false;
    });

});