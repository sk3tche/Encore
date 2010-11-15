#!/bin/sh
xbuild \
  /p:DocumentationFile="" \
  /p:DefineConstants="MONO" \
  /p:Configuration="Release|AnyCPU" \
  Trinity.Encore.sln
