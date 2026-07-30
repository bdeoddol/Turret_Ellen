#include <Arduino.h>
#include <Servo.h>
#include "Util.h" 
#include "SerialTurr.h"
//apparently you need to place the header/cpp files into a folder of the same name

Servo panServo;
Servo tiltServo;
Servo servo3;
SerialBuffer serBuff;
SerialCommand Cmd;
SerialTurrState servoState;
void setup()
{
  Serial.begin(9600);

  panServo.attach(7);
  tiltServo.attach(6);

  servoState.setPanPos(120);
  servoState.setTiltPos(39);
  panServo.write(servoState.getPanPos());
  tiltServo.write(servoState.getTiltPos());

  
}

void loop()
{

   if(Serial.available() > 0){ //available() returns num of bytes in the serial buffer 
    if(processByte(Serial.read(), serBuff) == true){ //process 1 byte in the serial buffer, return true if a string has been completed
      Cmd = parseSerialData(serBuff.getString()); //parse the completed string, copy the returned contents member by member 
      if(Cmd.getCmdAsChar() == 'C'){ 
        servoState.setPanPos(90);
        servoState.setTiltPos(90);

        panServo.write(servoState.getPanPos());
        tiltServo.write(servoState.getTiltPos());
      }
      else if(Cmd.getCmdAsChar() == 'S'){

      }
      else if (Cmd.getCmdAsChar() == 'M') {
        int NewPanPos = constrain(servoState.getPanPos() + (int)(Cmd.getPan() * Cmd.getGain()),10, 170);
        int NewTiltPos = constrain(servoState.getTiltPos() + (int)(Cmd.getTilt() * Cmd.getGain()),10, 170);
        servoState.setPanPos(NewPanPos);
        servoState.setTiltPos(NewTiltPos);

        panServo.write(servoState.getTiltPos());
        tiltServo.write(servoState.getTiltPos());
      }
      else if(Cmd.getCmdAsChar() == (NULL || 'P')){
        //do nothing, no movement
      }
    
    }
   }

}