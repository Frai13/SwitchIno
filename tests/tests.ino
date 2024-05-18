#include <Arduino.h>
#include <EasIno.h>
#include <SerialCom.h>

int NO_PIN = 5;
int NC_PIN = 6;

// Pull down resistor
int digital_no_val = LOW;
int digital_nc_val = LOW; 

EasIno* easino;
DataCom data_received;

void setup() {
    easino = new SerialCom(115200);
    easino->start();
    pinMode(NO_PIN, INPUT);
    pinMode(NC_PIN, INPUT);
}

void loop() {
    data_received = easino->receive();

    int digital_no_val_updated = digitalRead(NO_PIN);
    int digital_nc_val_updated = digitalRead(NC_PIN);
    if (digital_no_val_updated != digital_no_val) {
        digital_no_val = digital_no_val_updated;

        DataCom data = DataCom();
        data.setOperation("NO");
        data.setArgument(0, digital_no_val);
        easino->send(data);
    } else if (digital_nc_val_updated != digital_nc_val) {
        digital_nc_val = digital_nc_val_updated;

        DataCom data = DataCom();
        data.setOperation("NC");
        data.setArgument(0, digital_nc_val);
        easino->send(data);
    }
}