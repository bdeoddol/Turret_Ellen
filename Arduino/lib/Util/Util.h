#include <Arduino.h>
class SerialCommand{
    public: 
    SerialCommand(char cmd, int dX, int dY);
    SerialCommand();
    int getPan() const;
    int getTilt() const;
    char getCmdAsChar() const;
    private:
    int tilt;
    int pan;
    char SerCmd;


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

SerialCommand parseSerialData(String data);

void parseState(String serialLine);

bool processByte(byte dataByte, SerialBuffer& bufferClass);