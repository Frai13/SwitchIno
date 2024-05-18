# SwitchIno
SwitchIno is an app that handles the connection and disconnection of wires, in a similar way of relays do. It is based on [EasIno](https://github.com/Frai13/EasIno) and composed of an [Arduino](SwitchIno/) program and a [C#](SwitchInoApp/) app.

## Description
SwitchIno Arduino program manages 8 different outputs, 4 normally open (NO) and 4 normally closed (NC). [SwitchInoApp](SwitchInoApp/) allows user to interact with Arduino board using [EasIno](https://github.com/Frai13/EasIno) protocol.

## Components
1. **Arduino**:
    - Controller of the physical connections and disconnections of the wires.
    - Communication with C# using SerialPort and EasIno protocol.

2. **C# app and CLI**:
    - C# .NET Framework app that manages Arduino outputs.
    - Wrote using [EasIno API C#](https://github.com/Frai13/EasInoCSharpAPI).
    - Capability to work as an UI and as CLI. To see available commands type:
        > SwitchIno.exe --help