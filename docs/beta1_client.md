Beta1 Client
=============================================================================

Most things that apply to the alpha client are also applicable to the
iRO Beta1 client, which was released only three months after the alpha.
We won't repeat all of it here, so you should read the notes on the alpha
client as well.

Running the client
-----------------------------------------------------------------------------

It's currently unknown whether this is a common problem, but for at least
one of us, the client reset its 16-bit color compatibility settings every
time it's closed. Since the client will complain when this setting is not
set, it's easiest to create a BAT file to launch the client, where the
color mode is set before starting the Ragexe.exe.

```batch
set __COMPAT_LAYER=16BITCOLOR
start Ragexe.exe 1rag1
```
