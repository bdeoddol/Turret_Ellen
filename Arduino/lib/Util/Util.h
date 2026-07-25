#include <Arduino.h>

void readSerialLine(char* charString, int arrSize);

void parseSerialData(String data);


void parseState(String serialLine);

class SerialCommand{
    public: 
    SerialCommand(int dX, int dY);
    int tilt;
    int pan;


};
