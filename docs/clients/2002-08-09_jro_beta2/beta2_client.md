Beta2 Client
=============================================================================

The Japanese Beta2 client (dated 2002-08-09) is not officially supported
by the server yet, but basic compatibility to login and move around is
there.

The client is similar to the Alpha and Beta1 clients, but differs in the
general setup.

Running the client
-----------------------------------------------------------------------------

To launch the client, run the `Ragexe.exe` file in client folder with the
`1rag1` argument. Make sure to set the compatibility mode to 16-bit color,
as you will get an error otherwise.

You can also create a batch file to launch the client with these settings.

```batch
set __COMPAT_LAYER=16BITCOLOR
start Ragexe.exe 1rag1
```

Connecting to the server
-----------------------------------------------------------------------------

The Beta2 client reads the server IP and port from the `msgstringtable.txt`
file, found inside the `data.grf`. To make the client connect to your
server, you need to extract the file, edit it, and put it back in the
`data.grf`. The parameters should be found around line 456.

```text
455   %sのアース#
456   61.215.212.5#
457   6900#
458   http://www.ragnarokonline.jp#
```

Alternatively, you can modify the `Ragexe.exe` to have it read the file
directly from the disk instead of the `data.grf`. This can be accomplished
by opening the executable with any hex editor and replacing the following
bits.

Replace this:
```text
C745 E4 C0046000 6A 00
```
With this:
```text
C745 E4 C0046000 6A 01
```

And this:
```text
68 80000000 6A 02
```
With this:
```text
68 80000000 6A 03
```

Afterwards, the client will read the `msgstringtable.txt` file from the
same folder as the executable. You then only need to extract and edit the
file, without repacking the `data.grf`.
