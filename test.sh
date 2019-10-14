#!/bin/bash
 
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput='../../coverage.xml'

bash <(curl -s https://codecov.io/bash) -f "coverage.xml" -t ${codecov_token}
