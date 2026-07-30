#include <Arduino.h>

class SerialCommand{
    public: 
    SerialCommand(char cmd, int dX, int dY, int setGain);
    SerialCommand();
    int getPan() const;
    int getTilt() const;
    int getGain() const;
    char getCmdAsChar() const;
    private:
    int tilt;
    int pan;
    int gain;
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