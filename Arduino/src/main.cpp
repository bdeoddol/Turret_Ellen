#include <Arduino.h>
#include "Util.h" 
//apparently you need to place the header/cpp files into a folder of the same name

char commandStream[50];
String buff;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(8, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(6, OUTPUT);
  pinMode(5, OUTPUT);

  // digitalWrite(8,HIGH);
  // digitalWrite(7,HIGH);
  // digitalWrite(6,HIGH);
  // digitalWrite(5,HIGH);


  
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0){ //available() returns num of bytes in the serial buffer 
    digitalWrite(8,HIGH);
    digitalWrite(7,LOW);
    digitalWrite(6,HIGH);
    digitalWrite(5,LOW);
    delay(500);
    digitalWrite(8,LOW);
    digitalWrite(7,LOW);
    digitalWrite(6,LOW);
    digitalWrite(5,LOW);

    byte dataByte;
    dataByte = Serial.read();
    if(dataByte != \n){
      process
    }

    // readSerialLine(commandStream, 50);
    buff = commandStream;

    if(buff == "Idle"){ 
      digitalWrite(8,HIGH);
      digitalWrite(7,LOW);
      digitalWrite(6,LOW);
      digitalWrite(5,LOW);
      // delay(500);
    }
    else if(buff == "Tracking"){
      digitalWrite(8,LOW);
      digitalWrite(7,HIGH);
      digitalWrite(6,LOW);
      digitalWrite(5,LOW);
      // delay(500);
    }
    else if(buff == "Searching"){
      digitalWrite(8,LOW);
      digitalWrite(7,LOW);
      digitalWrite(6,HIGH);
      digitalWrite(5,LOW);
      // delay(500);
    }
    else if (buff == "Remote"){
      digitalWrite(8,LOW);
      digitalWrite(7,LOW);
      digitalWrite(6,LOW);
      digitalWrite(5,HIGH);
      // delay(500);
    }

  }
  // else{
  //   digitalWrite(8,LOW);
  //   digitalWrite(7,LOW);
  //   digitalWrite(6,LOW);
  //   digitalWrite(5,LOW);
  // }

}


