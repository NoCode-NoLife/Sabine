Sabine
=============================================================================

Sabine is an open-source MMORPG server, developed as a collaborative effort
to create a server software that gives users the chance to experience the
early versions of RO for fun and preservation, while giving developers the
oppurtunity to learn and hone their skills in a familar environment.

This project is very explicilty not about playing a game or competing
with any services provided by game developers or publishers, and we don't
endorse such actions. We're here to learn and create, not to steal or
destroy.

Compatibility
-----------------------------------------------------------------------------

Sabine is currently only compatible with the alpha client from 2001-08-30.
The server might offer limited support for other clients, such as iRO
Beta1 from 2002-02-20, but we aren't actively working on supporting them
just yet.

State of Development
-----------------------------------------------------------------------------

Sabine is in its early stages of development and is more prototype than
fully featured server. It's being worked on primarily as a side-project,
and while basic functionality is there, bugs and missing features are
to be expected.

The features that are explicitly working are as follows.
- Account creation and login
- Character management
- Inventory management
- Stat management
- NPC dialogs and shops
- Monster spawns
- Very basic combat (prototype)

Requirements
-----------------------------------------------------------------------------

Sabine is being developed in C# (.NET 8+) and uses a MySQL database for
its storage. As such, to use Sabine you will need the following:

- The .NET SDK (8+)
- A MySQL-compatible database server (MariaDB 10+ recommended)

On an up-to-date Windows system, the SDK will already be included,
so you only need to install a MySQL-compatible server. On Linux and
macOS, you will need to install the SDK as well.

For more detailed instructions, please refer to our beginner guide
found in the docs folder, at `docs/beginner_guide.md`.

Installation
-----------------------------------------------------------------------------

* Compile Sabine
* Run `sql/main.sql` to setup the database
* Copy `system/conf/database.conf` to `user/conf/`,
  adjust the necessary values and remove the rest.

Afterwards, you should be able to start Sabine via the provided scripts or
directly from the bin directories. If you need more assistance, head over
to our chat.

How to create an account
-----------------------------------------------------------------------------

For the moment, the easiest way to do so is to append a suffix to your
username on the login window, with either `_M` or `_F`, denoting which
sex your characters are going to be, and enter the password of your
choice in the password field. The server will then create a new account
with that name and password. On future logins you must ommit the suffix.

If you want to turn your accounts into GM accounts, change their `authority`
to 99 in the account database.

Contribution
-----------------------------------------------------------------------------

Check the file CONTRIBUTING.md for instructions on how you may contribute.

Links
-----------------------------------------------------------------------------

* GitHub: https://github.com/NoCode-NoLife/Sabine
* Chat: https://discord.gg/5sszEgw
