$(document).ready(function () {

    var tag = ''; // para teste, o valor oficial eh ''
    console.log("Variavel tag declarada!");
    setInterval(function requestObserver() {
        console.log("Inicio do intervalo! Tentar fazer a requisicao Ajax...");
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

                console.log("Sucesso na requisicao");

                $('#Rfid').val(tag);
                $('form').submit();
            },
            error: function (jqXHR, status) {

                $.ajax(requestObserver());
                console.log("Erro na requisicao: " + jqXHR);
            }
        });

        console.log("Fim da requisicao Ajax!");
        if (tag == '') {

            console.log("Recarregando a pagina...");
        }
    }, 3000);

});