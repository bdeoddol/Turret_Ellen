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
    int _tilt;
    int _pan;
    int _gain;
    char _SerCmd;


};

SerialCommand parseSerialData(String data);

class SerialTurrState{
    public:
    SerialTurrState();
    int getPanPos();
    int getTiltPos();
    void setPanPos(int value);
    void setTiltPos(int value);

    private:
    int _tiltPos;
    int _panPos;
};