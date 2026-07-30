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
  _SerCmd = NULL;
  _pan = 0;
  _tilt = 0;
}

SerialCommand::SerialCommand(char cmd, int dX, int dY, int setGain){
  _SerCmd = cmd;
  _pan = dX;
  _tilt = dY;
  _gain = setGain;
}

char SerialCommand::getCmdAsChar() const{
  return _SerCmd;
}

int SerialCommand::getPan() const{
  return _pan;
}

int SerialCommand::getTilt() const{
  return _tilt;
}

int SerialCommand::getGain() const{
  return _gain;
}

SerialTurrState::SerialTurrState(){
  _tiltPos = 0;
  _panPos = 0;
}

int SerialTurrState::getPanPos(){ return _panPos;}
int SerialTurrState::getTiltPos(){ return _tiltPos;}
void SerialTurrState::setPanPos(int value){_panPos = value; return;}
void SerialTurrState::setTiltPos(int value){_tiltPos = value; return;}