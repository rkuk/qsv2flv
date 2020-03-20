.PHONY: clean

qsv2flv: *.cs
	mcs -sdk:2 -out:$@ $^

clean:
	@-rm -rf qsv2flv *.temp
