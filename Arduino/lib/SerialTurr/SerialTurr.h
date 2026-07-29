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

SerialCommand parseSerialData(String data);

class SerialTurrState{
    public:
    SerialTurrState();
    int tiltPos;
    int panPos;
    int sens;
};