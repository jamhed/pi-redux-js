#!/bin/sh
cp -a bower_components/pi/sample ./
cd sample/
ln -s ../bower_components ./lib
ln -s ../bower_components/pi/conf ./
ln -s ../bower_components/pi/css ./
