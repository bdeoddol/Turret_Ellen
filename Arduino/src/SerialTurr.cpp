#include "SerialTurr.h"

SerialCommand parseSerialData(String data) {
  // Expected: [CMD]:[pan]x[tilt]x[gain]
  if (data.length() == 0) {
    return SerialCommand();
  }

  char cmd = data[0];
  int colonIDX = data.indexOf(':');
  int xIDX = data.indexOf('x');
  int scdxIDX = data.indexOf('x', xIDX + 1);

  if (colonIDX == -1 || xIDX == -1 || scdxIDX == -1) {
    return SerialCommand();
  }

  int pan = -(data.substring(colonIDX + 1, xIDX).toInt());  //we must invert bc the orientation of my turr is 180 = left and 0 = right,  invert by '-' and we get 180 = right, 0 = left
  int tilt = data.substring(xIDX + 1, scdxIDX).toInt();
  int gain = data.substring(scdxIDX + 1).toInt();

  return SerialCommand(cmd, pan, tilt, gain);
}

SerialCommand::SerialCommand(){
  SerCmd = NULL;
  pan = 0;
  tilt = 0;
}

SerialCommand::SerialCommand(char cmd, int dX, int dY, int setGain){
  SerCmd = cmd;
  pan = dX;
  tilt = dY;
  gain = setGain;
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

int SerialCommand::getGain() const{
  return gain;
}

SerialTurrState::SerialTurrState(){
  tiltPos = 0;
  panPos = 0;
}