Antichamber Save Watcher
========================

Tool for displaying Antichamber save file updates (collected guns, signs, pink cubes) in real time.

A (probably) up-to-date Win32 binary can be downloaded from https://sourceforge.net/projects/antichamber-sw/files/

Usage
-----

While you can run ASW with now command line arguments, it is recommended you specify the exact flags and (optionally) the path to Antichamber's save file you need. The possible arguments are as follows:

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

If no save file is specified, ASW defaults to watching ``C:\Program Files\Steam\SteamApps\common\Antichamber\Binaries\Win32\SavedGame.bin``. If no arguments at all are given, ASW defaults to just tracking signs.
