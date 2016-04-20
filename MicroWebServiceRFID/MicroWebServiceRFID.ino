#include <SPI.h>
#include <Ethernet.h>

#include<RFID.h> //inclui a biblioteca RFID

#define SS_ETH 10
#define SS_RFID 5
#define RST_RFID 9

RFID rfid(SS_RFID,RST_RFID); //cria objeto RFID

byte dado[] = {0,0,0,0,0};
// Entre com os dados do MAC e ip para o dispositivo.
// Lembre-se que o ip depende de sua rede local
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xEE };
IPAddress ip(192,168,25,101);
IPAddress gateway(192,168,25,1);
IPAddress subnet(255,255,255,0);
 
// Inicializando a biblioteca Ehternet
// 80 é a porta que será usada. (padrão http)
EthernetServer server(80);
int f=0;
void setup() {
  // Abrindo a comunicação serial para monitoramento.
  Serial.begin(9600);
  // Inicia a conexão Ethernet e o servidor:
  Ethernet.begin(mac, ip, gateway, subnet);
  server.begin();
  Serial.print("Servidor iniciado em: ");
  Serial.println(Ethernet.localIP());
  digitalWrite(SS_ETH, HIGH);
  digitalWrite(SS_RFID, LOW);   
  rfid.init(); //abrir biblioteca RFID.h e .cpp
}
 
void loop() { 
  if(rfid.isCard()){
    if(rfid.readCardSerial()){ 
      Serial.print("ID: "); 
      for(int k=0;k<5;k++){
        dado[k] = rfid.serNum[k]; 
        Serial.print(" "); 
        Serial.print(dado[k], HEX);
        if (dado[k]!= 0) f=1;
      }
    } 
    Serial.println(" "); 
    delay(500);
  }
  rfid.halt();

  // Se a tag for diferente de vazio, então eis o que acontece.
  if(f != 0){
    f=0;
    digitalWrite(SS_ETH, LOW);
    digitalWrite(SS_RFID, HIGH);
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
            //client.println("Refresh: 5");  // atualizar a página automaticamente a cada 5 segundos
            client.println();
            //client.print("<html><head><title>ArduinoRFID</title></head><body><input type='text' value='");
            client.write((char *)dado); // envio a tag como texto da resposta da requisição, teoricamente (até testar)
            //client.println("' /></body></html>");
            Serial.print("Respondendo ao cliente a Tag: ");
            Serial.println((char *)dado);
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
      //delay(3000); // tempo antes da limpeza da tag
      digitalWrite(SS_ETH, HIGH);
      digitalWrite(SS_RFID, LOW);
    }
  }
}

