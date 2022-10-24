#!/bin/sh
echo $0 $*
mydir=`pwd`
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$mydir/lib/
export SDL_VIDEODRIVER=mmiyoo
export SDL_AUDIODRIVER=mmiyoo
export EGL_VIDEODRIVER=mmiyoo
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
cd $(dirname "$0")
./SDL2_MiyooMiniExample
