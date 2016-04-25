/* Função para convertar texto em DEC abaixo */
function convertDec(input) {

    var dec = '';
    for (i = 0; i < input.length; i++) {
        dec += input[i].charCodeAt(0);
    }
    return dec;
}
/* Função para convertar texto em DEC acima */

/* Função para observar, via requisições em intervalos */
function observer() {

    var count = 0;
    setInterval(function request() {
        nocache = "&nocache=" + Math.random() * 1000000;
        var request = new XMLHttpRequest();
        request.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 200) {
                    if (this.responseText != null) {
                        if (window.location.href != 'http://theaccesscontrol.azurewebsites.net/Cadastro/Cadastrar')
                            $('#Rfid').val(convertDec(this.responseText));
                        else $('#tagrec').val(convertDec(this.responseText));
                        $('#load').css('display','block').css('text-align','center');
                        $('form').submit();
                    }
                }
            }
            count += 1;
            if (count < 300) {
                console.log('requisicao: ' + count +
                          '\nreadyState: ' + this.readyState +
                          '\nstatus: ' + this.status +
                          '\nresponseText: ' + convertDec(this.responseText));
            }
            if (count == 300) {
                console.clear();
                count = 1500;
                window.location.reload(false);
            }
        }
        request.open('GET', 'http://192.168.25.101/', true);
        request.send(null);
        setTimeout('request()', 1000);
    }, 1000);
}
/* Função para observar, via requisições em intervalos */

$(document).ready(function () {
    $.ajax(observer());
});