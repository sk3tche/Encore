#!/bin/sh
xbuild \
  /p:DocumentationFile="" \
  /p:DefineConstants="MONO" \
  /p:Configuration="Release" \
  /p:Platform="Any CPU" \
  Trinity.Encore.sln
