Features
=============================================================================

The features file in the data controls features that changed in Ragnarok
over time, such as the availability of certain items and jobs.

Hierarchy
-----------------------------------------------------------------------------

Consider the following snippet, with 3 features in a parent/child
relationship. `Beta1` is the overall parent, followed by its children
"Mage" and "Skills".

system/data/features.txt
```
{ name: "Beta1", enabled: true, children: [
	{ name: "Mage", enabled: true },
	{ name: "Skills", enabled: true },
]},
```

If you were to change the `enabled` value of `Beta1` to false, you would
disable not only `Beta1`, but also all of its children. This also applies
if you take `Beta1` and put it on its own, either further down in the system
features file, or in a separate features file in the user folder.

user/data/features.txt
```
{ name: "Beta1", enabled: false },
```

The above line would disable _all_ `Beta1` features from the first example.
If you wanted to disable all but one of them, you could first disable `Beta1`
and then enable the one you want. For example:

user/data/features.txt
```
{ name: "Beta1", enabled: false },
{ name: "Skills", enabled: true },
```

Keep in mind that every time you enable or disable a feature, you also
enable and disable all of its children, which makes order important.
If you first enabled "Skills" and then disabled `Beta1`, you would
effectively disable "Skills" again, because it's a child of `Beta1`.

Most commonly you will presumably enable entire updates or episodes at once,
disable some feature you don't like, or enable features of later updates,
and for these cases you should just cherry pick the specific features,
put them into the features file in the user folder, and remember to put
a feature's children after the feature itself, just like in the example
above.
