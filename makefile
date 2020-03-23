.PHONY: clean

qsv2flv: *.cs
	mcs -out:$@ $^

clean:
	@-rm -rf qsv2flv *.temp
