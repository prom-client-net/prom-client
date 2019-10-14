#!/bin/bash
 
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput='../../coverage.xml'

export CI="True"
export APPVEYOR="True"
export CODECOV_TOKEN=${codecov_token}

bash <(curl -s https://codecov.io/bash) -f 'coverage.xml' || echo 'Codecov failed to upload'
