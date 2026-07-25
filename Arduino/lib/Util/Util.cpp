#include "Util.h"

void readSerialLine(char* charString, int arrSize){
  char byte;
  int idx = 0;
  while(Serial.available() > 0){
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