Antichamber Save Watcher
========================

Tool for displaying Antichamber save file updates (collected guns, signs, pink cubes) in real time.

A (probably) up-to-date Win32 binary can be downloaded from [the Releases page](https://github.com/Crehl/antichamber-save-watcher/releases).

Usage
-----

ASW can now be configure in three ways:
* **Automatically** ``(recommended)`` - Just run it. If you have Steam or Antichamber running already, it'll find the path to the save file automatically. It will also prompt you for what you want to track - just press 'y' or 'n' and everything wil get setup.

* **Configuration file** - ASW will attempt to load configuration from a file named ``AntichamberSaveWatcher.ini``. If you create this file you can specify a path and what to track without having to do this every time you run it (unlike above). See below for an example configuration file.

* **Command line arguments** - You can pass several arguments into ASW through something such as a shortcut or straight form the command line. See below for a full list.

Example config file
-------------------
This is just an example. You don't need to specify everything, though this file has.

    [AntichamberSaveWatcher]
    Path=H:\Steam\SteamApps\common\Antichamber\Binaries\Win32\
    TrackSigns=true
    TrackGuns=false
    TrackCubes=false

Command line arguments
----------------------

Single letter arguments can be combined into one, e.g. ``-s -c -f "path"`` is equivalent to ``-scf "path"``.
The possible arguments are as follows:

    Usage:
      AntichamberSaveWatcher [arguments]
    
    Arguments:
      -s, --signs
        Displays any new signs collected
      
      -c, --cubes
        Displays any new pink cubes collected
    
      -g, --guns
        Displays any new guns collected
      
      -d, --debug
        Shows certain error messages in the console (you'll want this off unless you're having problems)
      
      -n, --no-resize
        Prevents the window from resizing itself on startup.
      
      -f <path>, --file <path>
        Watches the given save file for changes instead of the default path.

Default Behaviour
-----------------
If no save file is specified, ASW will attempt to find a running process matching either Steam or Antichamber (UDK). Failing that, ASW defaults to watching ``C:\Program Files (x86)\Steam\SteamApps\common\Antichamber\Binaries\Win32\SavedGame.bin``.
If no arguments at all are given, ASW defaults to just tracking signs.
