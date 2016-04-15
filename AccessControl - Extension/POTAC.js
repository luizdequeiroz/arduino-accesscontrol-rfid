$(document).ready(function () {

    var tag = '123456789'; // para teste, o valor oficial eh ''
    while (true) {
        // Consumir a WebService Arduino...
        $.ajax({
            type: "GET",
            url: "http://ip.aduino.ethernetshield.ip/", // endereco de teste para forcar o erro
            //data: "iAmTheExtension",
            timeout: 3000,
            contentType: "application/json; charset=utf-8",
            //dataType: "json",
            success: function (result, jqXHR) {
                // Interpretando retorno JSON...
                tag = JSON.parse(result);
            },
            error: function (jqXHR, status) {
                tag = '123456789'; // para teste, o valor oficial eh ''
            },
        });
        if (tag != '')
            break;
    }
    $('#Rfid').val(tag);
    $('form').submit();
});