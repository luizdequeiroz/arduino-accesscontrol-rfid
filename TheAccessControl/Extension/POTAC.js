/* Função para convertar texto em DEC abaixo */
function convertDec(input) {

    var dec = '';
    for (i = 0; i < input.length; i++) {
        dec += input[i].charCodeAt(0);
    }
    return dec;
}
/* Função para convertar texto em DEC acima */

function interrupt() { }

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
                        $('#load').css('display', 'block').css('text-align', 'center');
                        $('form').submit();
                    }
                }
            }
            count += 1;
            if (count < 100) {
                console.log('requisicao: ' + count +
                      '\nreadyState: ' + this.readyState +
                      '\nstatus: ' + this.status +
                      '\nresponseText: ' + convertDec(this.responseText));
            }
            if (count == 100) {
                console.clear();
                count = 500;
                window.location = 'http://theaccesscontrol.azurewebsites.net/';
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