#!/bin/bash
 
bash <(curl -s https://codecov.io/bash) -f "coverage.xml" -t ${codecov_token}
