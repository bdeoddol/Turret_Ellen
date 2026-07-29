#include <Arduino.h>
#include "Util.h" 
#include "SerialTurr.h"
//apparently you need to place the header/cpp files into a folder of the same name

SerialBuffer serBuff;
SerialCommand Cmd;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(8, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(6, OUTPUT);
  pinMode(5, OUTPUT);
  
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0){ //available() returns num of bytes in the serial buffer 
    if(processByte(Serial.read(), serBuff) == true){ //process 1 byte in the serial buffer, return true if a string has been completed
      Cmd = parseSerialData(serBuff.getString()); //parse the completed string, copy the returned contents member by member 
      if(Cmd.getCmdAsChar() == 'C'){ 
        digitalWrite(8,HIGH);
        digitalWrite(7,LOW);
        digitalWrite(6,LOW);
        digitalWrite(5,LOW);
      }
      else if(Cmd.getCmdAsChar() == 'P'){
        digitalWrite(8,LOW);
        digitalWrite(7,HIGH);
        digitalWrite(6,LOW);
        digitalWrite(5,LOW);
      }
      else if(Cmd.getCmdAsChar() == 'S'){
        digitalWrite(8,LOW);
        digitalWrite(7,LOW);
        digitalWrite(6,HIGH);
        digitalWrite(5,LOW);
      }
      else if (Cmd.getCmdAsChar() == 'M'){
        digitalWrite(8,LOW);
        digitalWrite(7,LOW);
        digitalWrite(6,LOW);
        digitalWrite(5,HIGH);
      }
      serBuff.clearString(); //clear the processed string 
    }
  }
}


