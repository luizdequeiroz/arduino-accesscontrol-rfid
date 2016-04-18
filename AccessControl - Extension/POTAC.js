/*
$(document).ready(function () {

    var i = 0;
    var tag = ''; // para teste, o valor oficial eh ''
    setInterval(function requestObserver() {
        // Consumir a WebService Arduino...
        $.ajax({
            type: "GET",
            url: "http://192.168.25.101/", // endereco de teste para forcar o erro
            //data: "iAmTheExtension",
            timeout: 3000,
            contentType: "text/html; charset=utf-8",
            //dataType: "json",
            success: function (result, jqXHR) {
                // Interpretando retorno JSON...
                tag = result;

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
*/

/* Função para convertar texto em DEC abaixo */
function convertDec(input) {
    
    var dec = '';
    for (i = 0; i < input.length; i++) {
        dec += input[i].charCodeAt(0);
    }
    return dec;
}
/* Função para convertar texto em DEC acima */

function observer() {

    var count = 0;
    setInterval(function request() {
        nocache = "&nocache=" + Math.random() * 1000000;
        var request = new XMLHttpRequest();
        request.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 200) {
                    if (this.responseText != null) {
                        $('#Rfid').val(convertDec(this.responseText));
                        $('form').submit();
                    }
                }
            }
            count += 1;
            if (count < 100) {
                console.log('requisicao: ' + count +
                      '\nreadyState: ' + this.readyState +
                      '\nstatus: ' + this.status +
                      '\nresponseText: ' + this.responseText);
            }
            if (count == 100) {
                console.clear();
                count = 500;
                /* Teste */
                $('#Rfid').val(123456789);
                $('form').submit();
                //window.location = 'http://theaccesscontrol.azurewebsites.net';
            }
        }
        request.open('GET', 'http://192.168.25.101/', true);
        request.send(null);
        setTimeout('request()', 1000);
    }, 1000);
}

$(document).ready(function () {
    $.ajax(observer());
});