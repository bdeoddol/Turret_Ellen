#include <Arduino.h>
#include "Util.h" //apparently you need to place the header/cpp files into a folder of the same name

char commandStream[50];
String buff;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(8, OUTPUT);
  pinMode(7, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:

  // digitalWrite(8, HIGH);
  // digitalWrite(7, HIGH);
  // delay(500);
  // digitalWrite(8, LOW);
  // digitalWrite(7, LOW);
  // delay(500);




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


