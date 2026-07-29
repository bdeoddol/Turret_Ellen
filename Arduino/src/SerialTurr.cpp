#include "SerialTurr.h"

SerialCommand parseSerialData(String data){
 // should be in format [SERIALCMD]:[XXXX]x[YYYY]
  char cmd = data[0];
  int colonIDX = data.indexOf(':');
  int xIDX = data.indexOf('x');

  int pan = -(data.substring(colonIDX+1, xIDX).toInt()); //we must invert bc the orientation of my turr is 180 = left and 0 = right,  invert by '-' and we get 180 = right, 0 = left
  int tilt = data.substring(xIDX+1).toInt();

  
  
  return SerialCommand(cmd, pan, tilt);
}

SerialCommand::SerialCommand(){
  SerCmd = 0;
  pan = 0;
  tilt = 0;
}

SerialCommand::SerialCommand(char cmd, int dX, int dY){
  SerCmd = cmd;
  pan = dX;
  tilt = dY;
}

char SerialCommand::getCmdAsChar() const{
  return SerCmd;
}

int SerialCommand::getPan() const{
  return pan;
}

int SerialCommand::getTilt() const{
  return tilt;
}

SerialTurrState::SerialTurrState(){
  tiltPos = 0;
  panPos = 0;
}