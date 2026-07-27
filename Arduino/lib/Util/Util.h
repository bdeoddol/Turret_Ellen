#include <Arduino.h>
class SerialCommand{
    public: 
    SerialCommand(int dX, int dY);
    int tilt;
    int pan;


};

class SerialBuffer{
    public: 
    SerialBuffer();
    bool appendChar(char byte, int bufferSize);
    const String& getString() const;
    void clearString();
    private:
    int idx;
    char charBuffer[50];
    String stringMsg;
};

void readSerialLine(char* charString, int arrSize);

void parseSerialData(String data);

void parseState(String serialLine);

bool processByte(byte dataByte, SerialBuffer& bufferClass);