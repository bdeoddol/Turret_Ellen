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

  servoState.panPos = 120;
  servoState.tiltPos = 39;
  servoState.sens = 5;

  panServo.write(servoState.panPos);
  tiltServo.write(servoState.tiltPos);

  
}

void loop()
{

   if(Serial.available() > 0){ //available() returns num of bytes in the serial buffer 
    if(processByte(Serial.read(), serBuff) == true){ //process 1 byte in the serial buffer, return true if a string has been completed
      Cmd = parseSerialData(serBuff.getString()); //parse the completed string, copy the returned contents member by member 
      if(Cmd.getCmdAsChar() == 'C'){ 
        panServo.write(90);
        tiltServo.write(90);

        servoState.panPos = 90;
        servoState.tiltPos = 90;
      }
      else if(Cmd.getCmdAsChar() == 'S'){

      }
      else if (Cmd.getCmdAsChar() == 'M') {
        servoState.panPos = constrain(servoState.panPos + (int)(Cmd.getPan() * Cmd.getGain()),10, 170);

        servoState.tiltPos = constrain(servoState.tiltPos + (int)(Cmd.getTilt() * Cmd.getGain()),10, 170);

        panServo.write(servoState.panPos);
        tiltServo.write(servoState.tiltPos);
      }
      else if(Cmd.getCmdAsChar() == (NULL || 'P')){
        //do nothing, no movement
      }
    
    }
   }

}