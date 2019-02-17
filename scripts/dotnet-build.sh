#!/bin/bash
MYGET_ENV=""
case "$TRAVIS_BRANCH" in
  "develop")
    MYGET_ENV="-dev"
    ;;
esac

dotnet build -c Release --no-cache --source https://www.myget.org/F/dnc-dshop/api/v3/index.json