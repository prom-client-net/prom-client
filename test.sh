#!/bin/bash
 
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

export CI='True'
export APPVEYOR='True'

bash <(curl -s https://codecov.io/bash) || echo 'Codecov failed to upload'
