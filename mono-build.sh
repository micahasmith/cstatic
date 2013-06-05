#! /bin/bash

#check for mono existance
command -v dmcs >/dev/null 2>&1 || { echo "I require mono but it's not installed. Aborting." >&2; exit 1; }


cp CStatic/packages/ServiceStack.Text.3.9.48/lib/net35/ServiceStack.Text.dll release/mono/
dmcs -r CStatic/packages/ServiceStack.Text.3.9.48/lib/net35/ServiceStack.Text.dll -target:exe -out:release/mono/cstatic.exe  `find CStatic/ -name "*.cs"`








#dmcs -pkg ../packages/ServiceStack.Text.3.9.48D  `find . -name "*.cs"`