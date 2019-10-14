#!/bin/bash

if ${APPVEYOR_REPO_TAG} == "true"  ; then
  echo "Pack for NuGet"
  dotnet pack ${APPVEYOR_BUILD_FOLDER}/src/Prometheus.Client -c Release --include-symbols --include-source --no-build -o artifacts/nuget
else
  echo "Pack for MyGet"
  dotnet pack ${APPVEYOR_BUILD_FOLDER}/src/Prometheus.Client -c Release --include-symbols --include-source --no-build --version-suffix build${APPVEYOR_BUILD_NUMBER} -o artifacts/myget
fi
