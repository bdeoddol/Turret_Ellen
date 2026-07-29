#include <Arduino.h>
#include <Servo.h>
#include "Util.h" 
#include "SerialTurr.h"
//apparently you need to place the header/cpp files into a folder of the same name

Servo servo1;
Servo servo2;
void setup()
{
  servo1.attach(7);
  servo2.attach(6);

  servo1.write(0);
  servo2.write(0);

  
}

void loop()
{
  for(int i = 0; i <= 180; i++){
    servo1.write(i);
    servo2.write(i);
    delay(15);
  }
  
  delay(500);
  
  for(int j = 180; j >= 0; j--){
    servo1.write(j);
    servo2.write(j);
    delay(15);
  }
  
  delay(3000); //end at 0
}