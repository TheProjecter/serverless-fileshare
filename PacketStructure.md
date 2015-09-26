# Details #
They packet for now is 1 Byte header + 524288 bytes of data.  Data size may need to be adjusted for speed.

First 1 byte defines what the rest of the packet is (file transfer,search, filelist etc) which define what type of packet it is.
First Byte Value:

> 0
If the first 1 byte is 0 then it means "File Transfer." If this is then the data is:

> First Byte=FileID

> Rest of bytes=raw file data




(1 byte) (1 byte) (524287 bytes)
> (FileType)(FileID) (DATA)