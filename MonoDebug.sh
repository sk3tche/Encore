#!/bin/sh
xbuild \
  /p:DocumentationFile="" \
  /p:DefineConstants="MONO,TRACE,DEBUG,CONTRACTS_FULL" \
  /p:Configuration="Debug|AnyCPU" \
  Trinity.Encore.sln
