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
        res = parseInt(choice);
        $('#loadbar').show();
        $('#quiz').fadeOut();
        var question = $("quizz")[0].id;
        console.log("Question" + question);
        setTimeout(function () {
            $('#quiz').show();
            $('#loadbar').fadeOut();
            /* something else */
        }, 1500);
        
        console.log(choice);
        console.log(res);

    });

    //$('input[name=q+]').val(res);
});	
