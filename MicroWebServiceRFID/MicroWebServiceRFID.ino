#include <SPI.h>
#include <Ethernet.h>

#include<RFID.h> //inclui a biblioteca RFID

#define SS_PIN 10 //Slave Select
#define RST_PIN 9

RFID rfid(SS_PIN,RST_PIN); //cria objeto RFID
String tag = ""; // Vai acumular a tag aqui.
 
// Entre com os dados do MAC e ip para o dispositivo.
// Lembre-se que o ip depende de sua rede local
byte mac[] = { 
  0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
IPAddress ip(192,168,1,101);
 
// Inicializando a biblioteca Ehternet
// 80 é a porta que será usada. (padrão http)
EthernetServer server(80);
 
void setup() {
 // Abrindo a comunicação serial para monitoramento.
  Serial.begin(9600);
  while (!Serial) {
    ; // esperar por porta serial para conectar. Necessário apenas para Leonardo
  }
  SPI.begin(); //não sei se é necessário, peguei com o exemplo
  
  // Inicia a conexão Ethernet e o servidor:
  Ethernet.begin(mac, ip);
  server.begin();
  Serial.print("Servidor iniciado em: ");
  Serial.println(Ethernet.localIP());
  
  rfid.init(); //abrir biblioteca RFID.h e .cpp
}
 
void loop() {
  byte status; // Não sei pra quê serve isso.
  if(rfid.isCard()){
    if(rfid.readCardSerial()){ 
      Serial.print("ID: "); 
      for(int k=0;k<5;k++){
        Serial.print(" "); 
        Serial.print(rfid.serNum[k],HEX);
        tag += (rfid.serNum[k], HEX); // Aqui eu acumulo, teoricamente (até testar), a tag RFID.
      }
      Serial.println("");
    } 
    delay(500);
  }
  rfid.halt();

  // Se a tag for diferente de vazio, então eis o que acontece.
  if(tag != ""){
    // Aguardando novos clientes;
    EthernetClient client = server.available();
    if (client) {
      Serial.println("Novo Cliente");
      // Uma solicitação http termina com uma linha em branco
      boolean currentLineIsBlank = true;
      while (client.connected()) {
        if (client.available()) {
          char c = client.read();
          Serial.write(c);
          // Se tiver chegado ao fim da linha (recebeu um novo 
          // Caractere) e a linha estiver em branco, o pedido http terminou,
          // Para que você possa enviar uma resposta
          if (c == '\n' && currentLineIsBlank) {
            // Envia um cabeçalho de resposta HTTP padrão
            client.println("HTTP/1.1 200 OK");
            client.println("Content-Type: text/html");
            client.println("Connection: close");  // a conexão será fechada após a conclusão da resposta
            client.println("Refresh: 5");  // atualizar a página automaticamente a cada 5 segundos
            client.println();
            client.println(tag); // envio a tag como texto da resposta da requisição, teoricamente (até testar)
            break;
          }
          if (c == '\n') {
            // Você está começando uma nova linha
            currentLineIsBlank = true;
          } 
          else if (c != '\r') {
            // Você recebeu um caracter na linha atual.
            currentLineIsBlank = false;
          }
        }
      }
      // Dar tempo ao navegador para receber os dados
      delay(1);
      // Fecha a conexão:
      client.stop();
      Serial.println("Cliente desconectado");
      delay(2000); // tempo antes da limpeza da tag
      tag = ""; // zera a tag para o próximo cartão
    }
  }
}
