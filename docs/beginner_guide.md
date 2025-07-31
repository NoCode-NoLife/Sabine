Getting Started
=============================================================================

This guide aims to get newcomers towards a running alpha server and
client as quickly and easily as possible, with the least amount of
client and server modifications possible. It's currently primarily
intended for Windows users, though the general process is the same
for other systems.

If you already have experience with other RO servers, such as eAthena
and its forks, the entire process will feel familiar and shouldn't take
longer than a couple of minutes.

Windows
-----------------------------------------------------------------------------

### Step 1 - Download Server

Either clone the repository using a Git-client of your choice, or download
it directly as a zip-file. Which option you choose should generally not
affect your experience, though using Git makes future updates easier.
If you've never used Git, however, downloading the server's source
code as-is is perfectly fine. On GitHub you can find the option to
do so semi-hidden behind the "Code" button on the repository's main
page.

### Step 2 - Compilation

Once you have downloaded and extracted the server's source code,
you need to open the file `Sabine.sln` in a C# IDE/editor of your
choice and compile it, meaning to turn the code into executables
that you can run to start the server.

The easiest option here is to download Visual Studio Community Edition,
a free IDE by Microsoft, which you can find here:
https://visualstudio.microsoft.com/vs/community/

If you just follow the setup process it should install everything
you will need. Just make sure that the components for ".NET Desktop
development" get installed.

Once complete, you can open the solution (`Sabine.sln`) in Visual
Studio and compile it by either pressing F6, or by clicking `Build`
in the menu bar and then the first option, `Build Solution`.

This process may take a couple seconds. Once complete, you should see
a new folder called `bin` in your Sabine folder, and a couple sub-
directories in, there should be several .exe, .dll and more files.

### Step 3 - Database

Before you can launch the server though, you need to set up a database,
so the server can save accounts and characters somewhere. For this you
need a MySQL-compatible server, such as MySQL or MariaDB. Which one
you choose doesn't really matter, and there are a plethora of options
for how to acquire a MySQL server.

Arguably one of the simplest solutions is to install a web development
environment for Windows, such as WAMP or XAMPP. These are bundles
that come with a web and database server, as well as applications
that let you work with the database. It's more than one would need
for running Sabine, but with their simple-to-use interfaces they
are a solid option.
https://www.apachefriends.org/

Another is to install just the database server, such as MariaDB:
https://mariadb.org/download/

This is a command line application that will usually always run
in the background somewhere, and you can connect to it via a MySQL
client, such as HeidiSQL, to manage the data.
https://www.heidisql.com/download.php

You can't really go wrong with either option. Regardless of which
you choose, download the respective installer and follow the setup
instructions. At the end of it you should be able to access the
database, either via PhpMyAdmin if you chose the first option,
opening it by clicking the Admin button for the database after
starting Apache and MySQL, or via HeidiSQL if you installed just
the database server, with whatever credentials you set during the
setup process.

In the database you must then choose to "run" or "import" a MySQL
file. Choose the `main.sql` from inside Sabine's `sql` folder and
execute it to import the basic database structure. This process
should be instantaneous, and you should be able to find the new
"sabine" database among the list of existing database. A refresh
might be necessary for it to appear though.

After you made it through this, there should be very few reasons
to ever directly work on the database again, as Sabine will apply
any potential updates to the database automatically.

The only thing left for the database is to configure the credentials
necessary to connect to it, if you haven't used the defaults, which
are usually username "root" with no password.

Go into the folder `user/conf/` and create a new file called
`database.conf`, or copy the existing `database-example.conf`
and rename it. Inside, set the username and password. For
example:
```text
db_user: root
db_pass: mypassword
```

Utilizing the `user` folder ensures that you'll be able to easily
update your server in the future, as you can download a new version
and simply copy over your user folder to preserve your changes,
with the user folder extending and overriding files in `system`.

To check if everything went well up to this point you can launch
the server by double-clicking the `start-all.bat` in Sabine's root
directory, which will start the auth, char, and zone servers. Three
command line windows should open, and after a couple seconds they
should all report that the server is ready and listening.

If there are any errors or warnings, something went wrong and you
might have to try again or seek additional guidance.

### Step 4 - Client

The last step before you can finally play is to download and prepare
the client. The alpha client can be downloaded from one of the following
sources.

- https://www.castledragmire.com/ragnarok/downloads.php
- https://archive.org/details/ro-alpha-client-english

Once you have it, you should apply one semi-mandatory data modification,
in the form of an updated item name table, which is necessary for the
client to support all available items. You can find the files in
`docs/clients/2001-08-30_iro_alpha/mods/files`. Just copy the `data`
folder inside into your client folder as-is.

**Folder structure example**
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

We also recommend you install "dgVoodoo", which is an application that
updates certain aspects of old games so they run better on modern systems.
Without this, the alpha client can feel a bit laggy and sluggish. You can
find it here: https://dege.freeweb.hu/dgVoodoo2/dgVoodoo2/

Simply download the latest version and extract the files from the
`MS/x86/` folder into your client folder.

**Folder structure example**
```
|- D3D8.dll
|- D3D9.dll
|- D3DImm.dll
|- DDraw.dll
|...
|- RagExe.exe
|- Ragnarok.exe
|- Setup.exe
```

To launch the client, you will need to execute it with a special parameter.
For this, create a shortcut to the `RagExe.exe`, right-click it, choose
`Properties`, and append this to the target: `-ragpassword`

Full target example: `RagExe.exe -ragpassword`

Alternatively, you can use a .bat file for the same purpose or launch the
client directly via command line.

The final puzzle piece is to tell the client where to connect to, because
it will try to connect to a long defunct official server otherwise. The
easy way to do so, and the recommended one, due to additional benefits,
is the use of "HookCat", which can be downloaded here:
https://github.com/exectails/HookCatRO

This is a tool that makes some changes to the client's behavior and
improves the user experience, including the ability to set the address
to connect to. If you don't want to use it, read on, otherwise skip
to the next step.

To manually change the connection address, you need to modify the
RagExe.exe with a hex editor, such as "HxD", which you can download
here:
https://mh-nexus.de/en/hxd/

This step might look complicated, but it's rather simple. Just instruct
the editor to find and replace all instances of these bytes:
```text
32 31 31 2E 32 33 39 2E 31 32 33 2E 31 36 38
```
with these:
```text
31 32 37 2E 30 2E 30 2E 31 00 00 00 00 00 00
```
similar to how you would replace text inside a text editor. This will
make the client connect to your localhost IP, `127.0.0.1`, instead of
the original, `211.239.123.168`.

### Step 5 - Play

With this, everything should be in place, and you should be able to
finally launch the client by double-clicking the shortcut you created.
Once you arrive at the login window, create an account by choosing any
name and password. Just make sure to append either `_M` (Male) or `_F`
(Female) to the username, which will instruct the server to create
an account with the chosen gender, determining the sex of the characters
on that account.

If you successfully followed this guide and got into the game, you can
now experience our best approximation of what the RO alpha was like.
Have fun!

### Optional - Beta1 and Beyond

The general setup process for the server is mostly the same for the
Beta1 and later clients, with a few exceptions.

1) You have to modify the client version in `system|user/conf/version.conf`
   to match the client you'd like to use.
2) You have to change the connection address in the client, as described
   above, to connect to the server. For other clients HookCat is not
   available yet and you will have to use a hex editor.
3) Use different client startup parameters.

#### Beta 1

Replace
```text
32 31 31 2E 32 33 39 2E 31 36 31 2E 37 34
```
with
```text
31 32 37 2E 30 2E 30 2E 31 00 00 00 00 00
```
and instead of `-ragpassword`, start the client with
```text
1rag1
```
