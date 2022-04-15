Sabine
=============================================================================

Sabine is an open-source MMORPG server software for the alpha client of
the english version of RO from October 2001. It's being developed for
educational purposes, learning about programming, MMORPGs, maintaining
huge projects, working with other people, and improving knowledge.
It's not about playing a game or competing with any services provided
by Gravity or its partners.

Sabine is completely free and licensed under the GNU GPL. As such,
every user is free to use Sabine and choose how to use it, in the
context of its license.

Compatibility
-----------------------------------------------------------------------------
Sabine is currently only compatible to the ‎alpha client from 2001-08-30.

Requirements
-----------------------------------------------------------------------------
To *run* Sabine, you need
* .NET 4.8
* MySQL 5 compatible database

To *compile* Sabine, you need
* C# 7 compiler, such as:
  * [Visual Studio](http://www.visualstudio.com/en-us/products/visual-studio-express-vs.aspx) (2017 or later)
  * [Monodevelop](http://monodevelop.com/) (With Mono version 5 or greater)

Installation
-----------------------------------------------------------------------------
* Compile Sabine
* Run `sql/main.sql` to setup the database
* Copy `system/conf/database.conf` to `user/conf/`,
  adjust the necessary values and remove the rest.

Afterwards, you should be able to start Sabine via the provided scripts
or directly from the bin directories.

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
Check the file CONTRIBUTING.md for information on how you may contribute.
