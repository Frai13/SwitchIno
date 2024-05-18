#ifndef OUTPUT_H
#define OUTPUT_H

#include <Arduino.h>


class Output
{
private:
  int mGpio;
  bool mValue;
  bool mIsInverted;

public:
  char* mName;

  Output() {}
  Output(char* mName, int gpio, bool default_value);
  int set(bool value);
  bool get();
  
};

#endif  // OUTPUT_H