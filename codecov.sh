 #!/bin/bash

curl -s https://codecov.io/bash > codecov-bash
chmod +x codecov-bash
./codecov-bash -f "coverage.xml" -t ${env:codecov_token} 
