#!/bin/sh
cd CarRent/carrent.frontend
npm install
npm run build
mkdir ../CarRent.Source/bin/Release/netcoreapp3.1/wwwroot
cp -R ./dist/* ../CarRent.Source/bin/Release/netcoreapp3.1/wwwroot
