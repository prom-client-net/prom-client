#!/bin/bash
 
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput='../../coverage.xml'
