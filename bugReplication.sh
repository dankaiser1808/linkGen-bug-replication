#!/bin/bash

randomNumber=$RANDOM
postResponse=$(curl -X POST https://localhost:7022/odata/Customers -H "Content-Type: application/json" -d "{\"Id\": \"${randomNumber}/Foo\",\"Name\": \"Bar\"}" -D- -s -o /dev/null)

location=$(echo "$postResponse" | grep -Fi Location | awk '{print $2}' | tr -d '\r')

echo "Location header value from response: $location"
echo "--------------------------------------"

getResponse=$(curl -X GET $location -H "Content-Type: application/json" -s -o /dev/null -w "%{http_code}" 2>&1)
echo "Get request to location results in: $getResponse"

echo "--------------------------------------"
echo "Location header should be fixed to: https://localhost:7022/odata/Customers('${randomNumber}%2FFoo')"
echo "--------------------------------------"

getResponse=$(curl -X GET "https://localhost:7022/odata/Customers('${randomNumber}%2FFoo')" -H "Content-Type: application/json" -s -o /dev/null -w "%{http_code}" 2>&1)
echo "Get request with fixed location results in: $getResponse"
