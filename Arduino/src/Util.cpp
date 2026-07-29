#include "Util.h"

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


SerialBuffer::SerialBuffer(){
  idx =  0;
  stringMsg = "";
}

const String& SerialBuffer::getString() const{
  return stringMsg;
}

void SerialBuffer::clearString(){
  stringMsg = "";
  return;
}

bool SerialBuffer::appendChar(char byte, int bufferSize){
  if(idx < bufferSize - 1){
    charBuffer[idx] = byte; 
    idx++;
    if(byte == '\0'){idx = 0; stringMsg = charBuffer; return true;}
  }
  else{charBuffer[bufferSize-1] = '\0'; idx = 0; return false;}

  return false;
}


bool processByte(byte dataByte, SerialBuffer& bufferClass){

  if(dataByte == '\r'){
    return false;
  }
  else if(dataByte == '\n'){
    return bufferClass.appendChar('\0', 50);
  }
  else{
    return bufferClass.appendChar(dataByte, 50);
  }
}




