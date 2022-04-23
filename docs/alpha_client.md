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

Differences
-----------------------------------------------------------------------------

There are certain differences between the alpha and newer versions of
the game, some of which might seem like bugs or errors. In this section
we'll briefly discuss these, to avoid confusion.

### Accessories

Usually, you're able to equip accessories in an accessory slot of your
choosing, but in the alpha, the client will always send an unequip packet
for the accessory slots if they're in use already. You can't ignore this
packet, because that would interfer with normal unequipping, so you have
to do it, which means that you can't equip two accessories at once if
they're marked to go in "both" slots. This leads us to believe that
accessories had dedicated slots they were going in, such as rings going
in Accessory1 and necklaces in Accessory2. From screen shots we know
that it was possible to equip two accessories, and this implementation
made the most sense to us. That's why you're limited in where you can
equip accessories on this server if you're using the alpha client.

### Next/Close

In the alpha, dialogues have only one possible button: "OK". There are
no separate Next and Close buttons, and the client will always show the
one button. Further, the "Close" function closes the dialog right away,
unlike newer versions, where it displays a Close button before closing
the dialog window.

Additionally, it was possible to cancel a dialog at any time by clicking
anywhere outside the dialog window. Scripters need to be aware of this,
to not accidentally close off some dialog option, because they assumed
the dialog would go from start to finish without interruptions.

### Stats

Unfortunately, there's very few screen shots, videos, and information
about stats during the alpha. A few things we've been able to figure
out, however. For example, Novices didn't get any Zeny when they first
started, characters had a much lower maximum weight, and the stats
were different in general. Don't be surprised when your stats are
different from what you're used to. That being said, the formulas
we're using are largely guess-work, based on the limited resources
we had, and if something seems wrong, do report it.

Monsters are even more difficult to nail down, because we don't have
any information about their exact stats. The MvPs in particular were
presumably much weaker than in later versions, because first class
characters had to be able to kill them, but without any official
information, we can't say what their stats were for sure.

### Item Descriptions

Unlike newer clients, the alpha client doesn't have any item descriptions
in its data. Instead, it sends a request to the server to receive them.
This server currently auto-generates the descriptions based on the items'
stats, which is why they're a little sparse.

What's interesting about the descriptions, is that they seem to not
support line-breaks. That means you can't create clean key:value lines,
like this:

```
Attack: 1~2
Weight: 10
Type: Foobar
```

The alpha client displays it all in one line. This might indicate
that items didn't have the detailed descriptions we know today,
but that they just described the items.
