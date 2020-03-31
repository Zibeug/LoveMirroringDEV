$(function () {
    var res = 0;
    var loading = $('#loadbar').hide();
    $(document)
        .ajaxStart(function () {
            loading.show();
        }).ajaxStop(function () {
            loading.hide();
        });

    $("label.btn").on('click', function () {
        
        var choice = $(this).find('input:radio').val();
        res = res + parseInt(choice);
        $('#loadbar').show();
        $('#quiz').fadeOut();
        setTimeout(function () {
            $('#quiz').show();
            $('#loadbar').fadeOut();
            /* something else */
        }, 1500);
        
        console.log(choice);
        console.log(res);

        $(this).find('#score').val(res);
    });

    $('#score').val(res);
});	
