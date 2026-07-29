#include <Arduino.h>

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

bool processByte(byte dataByte, SerialBuffer& bufferClass);