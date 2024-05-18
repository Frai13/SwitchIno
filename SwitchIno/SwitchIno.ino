#include <Arduino.h>
#include <EasIno.h>
#include <SerialCom.h>
#include "Output.h"

#define VERSION "v1.0.0"

EasIno* easino;
DataCom data_received;

Output* outputs[] = {
  new Output("NC1", 5, true),
  new Output("NC2", 6, true),
  new Output("NC3", 7, true),
  new Output("NC4", 8, true),
  new Output("NO1", 9, false),
  new Output("NO2", 10, false),
  new Output("NO3", 11, false),
  new Output("NO4", 12, false)
};


void setup() {
  easino = new SerialCom(115200);
  easino->start();
}

void loop() {
  data_received = easino->receive();
  if (strcmp(data_received.operation, "SI_VER") == 0) {
    DataCom data = DataCom();
    data.setOperation("SI_VER");
    data.setArgument(0, VERSION);
    easino->send(data);
    delay(100);
  } else if (strcmp(data_received.operation, "SET") == 0) {
    char* gpio = data_received.args[0];
    int value = atoi(data_received.args[1]);

    int index_find = search_output(gpio);
    if (index_find < 0) {
      send_easino("SET", -1, -1);
    } else if (value != 0 && value !=1) {
      send_easino("SET", -1, -1);
    } else {
      outputs[index_find]->set(value);
      send_easino("SET", gpio, value);
    }
  } else if (strcmp(data_received.operation, "GET") == 0) {
    char* gpio = data_received.args[0];

    int index_find = search_output(gpio);
    if (index_find < 0) {
      send_easino("GET", -1, -1);
    } else {
      bool value = outputs[index_find]->get();
      send_easino("GET", gpio, value);
    }
  }
}

int search_output(const char* gpio)
{
  const int arr_size = sizeof(outputs) / sizeof(outputs[0]);
  for (int i=0; i < arr_size; i++) {
    if (strcmp(outputs[i]->mName, gpio) == 0) {
      return i;
    }
  }

  return -1;
}

void send_easino(const char* op, const char* gpio, int value)
{
  DataCom data = DataCom();
  data.setOperation(op);
  data.setArgument(0, gpio);
  data.setArgument(1, value);
  easino->send(data);
  delay(100);
}
