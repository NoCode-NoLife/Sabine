Beta1 Client
=============================================================================

Most things that apply to the alpha client are also applicable to the iRO
Beta1 client, which was released only three months after the alpha. Please
refer to the alpha client documentation for general compatibility and
modification notes.

Startup
-----------------------------------------------------------------------------

The Beta1 client uses a different startup parameter than the alpha client.
To launch it at all, you need to execute the `Ragexe.exe` with the `1rag1`
parameter, instead of `-ragpassword`.

```text
Ragexe.exe 1rag1
```

Additionally, the hex code modification to change the connection address
is slightly different.

Replace
```text
32 31 31 2E 32 33 39 2E 31 36 31 2E 37 34
```
with
```text
31 32 37 2E 30 2E 30 2E 31 00 00 00 00 00
```
to make the client connect to your local server (127.0.0.1).

Compatibility Mode
-----------------------------------------------------------------------------

It's currently unknown whether this is a common problem, but for at least
one of the server devs, the client reset its 16-bit color compatibility
settings every time it's closed. Since the client will complain when this
setting is not set, it's easiest to create a BAT file to launch the
client, where the color mode is set before starting the Ragexe.exe.

```batch
set __COMPAT_LAYER=16BITCOLOR
start Ragexe.exe 1rag1
```

Headgears
-----------------------------------------------------------------------------

While the client contains various headgears that would typically go into
the middle or upper head slots, and even though it displays the slots in
the UI, it appears technically incapable of actually using them, since
the potential slots are sent as a byte flag bitmask with a max value of
0xFF, while the upper and mid slots use 0x100 and 0x200 respectively.
Unless there's some trickery going on, we have to assume only lower
headgears were actually working in Beta1.
