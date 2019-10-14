#!/bin/bash
 
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

export CI='True'
export APPVEYOR='True'
export CODECOV_TOKEN=${codecov_token}

bash <(curl -s https://codecov.io/bash) || echo 'Codecov failed to upload'
