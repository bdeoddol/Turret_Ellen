#include "Scripts.h"
#include "Util.h"

void verifySerialTransmissionSetUp(){
    Serial.begin(9600);
    pinMode(8, OUTPUT);
    pinMode(7, OUTPUT);
}
void verifySerialTransmissionLoop(){
    char commandStream[50];
    String buff;


    if(Serial.available() > 0){
    digitalWrite(8, HIGH);
    digitalWrite(7, HIGH);
    delay(500);

    readSerialLine(commandStream, 50);
    buff = commandStream;
    
    if(buff == "hello world"){
      digitalWrite(8, HIGH);
      digitalWrite(7, LOW);
      delay(500);

    }
    else{
      digitalWrite(8, LOW);
      digitalWrite(7, HIGH);
      delay(500);
    }
  }

  digitalWrite(8, LOW);
  digitalWrite(7,LOW);
}