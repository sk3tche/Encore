#!/bin/bash

xbuild \
  /p:DocumentationFile="" \
  /p:DefineConstants="MONO,TRACE,DEBUG,CONTRACTS_FULL" \
  /p:Configuration="Debug" \
  /p:Platform="Any CPU" \
  Trinity.Encore.sln
