#!/bin/sh
xbuild /property:DocumentationFile="" /property:DefineConstants="TRACE;DEBUG;CONTRACTS_FULL;MONO" Trinity.Encore.sln
