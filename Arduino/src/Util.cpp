#include "Util.h"

SerialCommand::SerialCommand(int dX, int dY){
  tilt = dY;
  pan = dX;
}


void readSerialLine(char* charString, int arrSize){ 
  //this loop uses Serial.Read() in a loop to build the incoming serial data. It is not error-proof and thus not used
  char byte;
  int idx = 0;
  //using serial.available() is not ideal because serial information comes byte-by-byte.
  //It's possible when we iterate fast enough such that when we finish a loop, the msg is incomplete and it appears as if there are no bytes available in the serial buffer, and we exit unintentionally
  while(Serial.read() != '\n'){ 

    if(idx >= arrSize - 1){charString[arrSize - 1] = '\0'; break;}
    byte = Serial.read();
    if(byte == '\n'){
      break;
    }
    else{
      charString[idx] = byte;
      idx++;      
    }
  }

  charString[idx] = '\0';
}



void processByte(byte dataByte){
  if (dataByte == '\n'){

  }
}

void parseSerialCommand(String data){
 // should be in format XXXX:YYYY
}

void parseState(String serialLine){

}


