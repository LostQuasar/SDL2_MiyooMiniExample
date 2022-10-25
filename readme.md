# SDL2 C# Example

This repository shows the basic layout of a C# SDL2 application for the Miyoo Mini

### Advantages of using C# and SDL2

runs on both windows and Miyoo for quicker debugging and development.

### Publish Settings

- Runtime: linux-arm
- Deployment mode: Self-contained
- File publish options:
  - Publish single file
  - Trim unused code

### How to run on Windows

- Compile [SDL2-CS](https://github.com/flibitijibibo/SDL2-CS)
- Add as a dependency in Visual Studio
- Download [SDL2](https://github.com/libsdl-org/SDL/releases/tag/release-2.24.1)
- Place SDL2.dll into `/bin/Debug/net6.0/`
- Run

### How to run on Miyoo

- Publish to folder
- Copy files from the folder to `SDCARD/App/SDL_Mini_Example/`

## Hacks and their reasoning

`DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1`

- Prevents dotnet from complaining about globalization library

`export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$mydir/lib/`

- Appends required libraries to the required enviroment variable

Using SDL2 as a frame buffer

- Unfortunately, all of SDL2 functionality has not been completed inside of [Steward-Fu](https://steward-fu.github.io/website/index.htm)'s port of SDL2 however enough exits to create C# programs.

## Using other libraries

SDL2_ttf and SDL2_image do work and have been tested although they will require additional libraries to work. I recommend downloading [Parayste](https://github.com/steward-fu/miyoo-mini/releases/) to obtain the libraries required. SDL2-gfx will not work as the graphics cannot be drawn to a surface as far as I know.
