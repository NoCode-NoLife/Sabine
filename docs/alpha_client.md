Alpha Client
=============================================================================

To the best of our knowledge, there is only one alpha client publically
available, the iRO client from 2001-08-30. It can be downloaded at the
following address.

https://www.castledragmire.com/ragnarok/downloads.php

Compatibility
-----------------------------------------------------------------------------

Sabine was primarily developed for use with this client, and it generally
works relatively well under modern operating systems, such as Windows 10.
However, there are a few recommended modifications that should be applied,
to make it run even better.

### dgVoodoo

Using dgVoodoo with the client makes it run much smoother and fixes the
mouse lag that is usually present when running the client with Windows 10.
The program is very simple to use, and after just copying a few DLLs to
the alpha client folder, it runs much better.

### Font

Due to anti-aliasing problems, some in-game text elements have a pink
"border" around them. This can be fixed by patching the RagExe.exe,
either at run-time or upfront. Simply modify the GetFontA calls, to
use `3` as the second to last argument, which makes it create non-anti-
aliased fonts.

### Alternatives

If you're open to using other systems than Windows 10+, there are
alternatives where the alpha clients runs well without modifications.
Namely, you could use a classic Windows, such as 98, for which the
game was made, or you could run the game under Linux/Wine, which is
also able to run the client without graphical glitches or performance
issues.

The easiest way, however, is to simply get a memory patcher or a
patched exe, together with dgVoodoo.

Other Issues
-----------------------------------------------------------------------------

### Item Names

Early clients used the item's names as ids to identify items by, which
causes several issues, such as not being able to change item names on
the server without also having to change them on the client. The biggest
issue, however, is that the official itemnametable.txt in the alpha
client contains item names that are too long for char arrays they need
to fit in in the packets, and can't actually be used. The client we have
is presumably missing patches to fix these issues. If we wanted to use
an unmodified alpha client, we would either be unable to use many items,
or we would need to send the Korean names to the client, which is not
very helpful for English-speaking players. Not to mention that the
client can't display Korean.

For this reason, we decided to use fixed item names for Sabine's item
data, which the client must use as well, to be able to find the items'
sprites. Simply open the files folder and copy the data folder to your
alpha client folder. It prioritizes the data folder by default and
will load the item names from there.

```
|- bgm\
|- data\
  |- itemnametable.txt
|- data.grf
|...
|- RagExe.exe
|- Ragnarok.exe
|- Setup.exe
```

If you don't use this name table, you will likely experience errors,
because the client will not be able to find certain item sprites.
