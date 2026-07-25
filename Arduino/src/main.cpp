#include <Arduino.h>

// put function declarations here:
// int myFunction(int, int);


void setup() {
  // put your setup code here, to run once:
  pinMode(8, OUTPUT);
  pinMode(7, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  String msg;
  if(Serial.available() > 0){
    digitalWrite(8,HIGH);
    digitalWrite(7,HIGH);
    delay(500);

    msg = Serial.readString();
    msg.trim();
    if(msg == "hi"){
      digitalWrite(8, HIGH);
      digitalWrite(7,LOW);
      delay(500);
    }
    else{
      digitalWrite(8, LOW);
      digitalWrite(7, HIGH);
      delay(500);
    }

    digitalWrite(8,LOW);
    digitalWrite(7,LOW);



  }



    // digitalWrite(8,HIGH);
    // digitalWrite(7,HIGH);
  // digitalWrite(8, HIGH);
  // digitalWrite(7, HIGH);
  // delay(500);
  // digitalWrite(8, LOW);
  // digitalWrite(7, LOW);
  // delay(500);
}


// put function definitions here:
// int myFunction(int x, int y) {
//   return x + y;
// }