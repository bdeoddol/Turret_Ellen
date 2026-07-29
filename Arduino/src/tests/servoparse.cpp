#include <Arduino.h>
#include <Servo.h>
#include "Util.h" 
#include "SerialTurr.h"
//apparently you need to place the header/cpp files into a folder of the same name

Servo servo1;
Servo servo2;
Servo servo3;
SerialBuffer serBuff;
SerialCommand Cmd;
void setup()
{
  Serial.begin(9600);
  DDRB |= 0x80;
  DDRB |= 0x40;
  DDRB |= 0x20;
  
  servo1.attach(7);
  servo2.attach(6);
  servo3.attach(5);

  servo1.write(90);
  servo2.write(90);
  servo3.write(180);
  
}

void loop()
{

   if(Serial.available() > 0){ //available() returns num of bytes in the serial buffer 
    if(processByte(Serial.read(), serBuff) == true){ //process 1 byte in the serial buffer, return true if a string has been completed
      Cmd = parseSerialData(serBuff.getString()); //parse the completed string, copy the returned contents member by member 
      if(Cmd.getCmdAsChar() == 'C'){ 
        servo1.write(90);
        servo2.write(90);
      }
      else if(Cmd.getCmdAsChar() == 'P'){

      }
      else if(Cmd.getCmdAsChar() == 'S'){

      }
      else if (Cmd.getCmdAsChar() == 'M'){

      }
      serBuff.clearString(); //clear the processed string 
    }
  }

}