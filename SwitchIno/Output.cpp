#include "Output.h"

Output::Output(char* name, int gpio, bool default_value)
{
  pinMode(mGpio, OUTPUT);

  mName = name;
  mGpio = gpio;
  mValue = default_value;
  mIsInverted = default_value;
}
  
int Output::set(bool value)
{
  if (value == mValue) {
    return 1;
  }
  mValue = value;
  digitalWrite(mGpio, value ^ mIsInverted);
  return 0;
}
  
bool Output::get()
{
  return mValue;
}