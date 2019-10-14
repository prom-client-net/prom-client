#!/bin/bash
 
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput='../../coverage.xml'
echo '1'
echo ${codecov_token}
echo '2'
echo ${env:codecov_token}
bash <(curl -s https://codecov.io/bash) -f "coverage.xml" -t ${codecov_token}
