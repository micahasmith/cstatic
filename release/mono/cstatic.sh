#! /bin/bash

# THIS DOESNT WORK YET

for i in $*;
do
    params=" $params $d/$i"
done

mono cstatic.exe $params