#!/bin/sh

cd $1

for f in `find bin -not -iname "$2.*" -and -type f`
do
	rm -v $f
done

for f in `find obj -iname "*.dll" -or -iname "*.pdb"`
do
	rm -v $f
done


