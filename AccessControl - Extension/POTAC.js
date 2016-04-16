$(document).ready(function () {

    var i = 0;
    var tag = ''; // para teste, o valor oficial eh ''
    setInterval(function requestObserver() {
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

                $('#Rfid').val(tag);
                $('form').submit();
            },
            error: function (jqXHR, status) {

                i += 1;
                if (i == 500) window.location = "http://theaccesscontrol.azurewebsites.net/";
                $.ajax(requestObserver());
            }
        });
    }, 3000);

});