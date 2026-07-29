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


  
}

void loop()
{

  servo1.write(90);
  servo2.write(90);
  delay(2000);

  servo1.write(0);
  servo2.write(0);
  delay(2000);

  servo1.write(90);
  servo2.write(90);
  delay(2000);

  servo1.write(180);
  servo2.write(180);
  delay(2000);

  servo1.write(90);
  servo2.write(90);
  delay(2000);


}