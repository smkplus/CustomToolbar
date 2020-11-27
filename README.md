# CustomToolbar
based on this [marijnz unity-toolbar-extender](https://github.com/marijnz/unity-toolbar-extender). 
![image](https://user-images.githubusercontent.com/16706911/100000419-cff31e00-2dd6-11eb-9a4b-8379e3a7cc50.jpg)



### Why use CustomToolbar?
This custom tool helps you to test and develop your game easily

____________
Scene selection dropdown to open scene in editor. Scenes in build have unity icon while selected and appear above splitter in list

![image](_readme/SceneSelect.jpg)
____________

when you want to clear all playerprefs you have to follow 3 step:

![image](https://user-images.githubusercontent.com/16706911/68548191-52dd4c80-03ff-11ea-85b6-e9899ab04c34.jpg)

but you can easily Clear them by clicking on this button:

![image](_readme/btnClearPrefs.jpg)
____________

another button relevant to saving is this button that prevents saving during the gameplay. because sometimes you have to Clear All playerprefs after each test so you can enable this toggle:

Enable Playerprefs:

![image](_readme/btnDisablePrefs.jpg)

Disable Playerprefs:

![image](_readme/btnDisablePrefsInactive.jpg)
____________

you can restart the active scene by this button:

![image](_readme/btnRestartScene.jpg)
____________

suppose you want to test your game so you should start game from scene 1(Menu):

![image](https://user-images.githubusercontent.com/16706911/68548295-8371b600-0400-11ea-8737-a9da3d555df0.png)

you have to find scene 1 (Menu):

![image](https://user-images.githubusercontent.com/16706911/68548309-c2a00700-0400-11ea-9740-128368bd801a.png)

then you should start the game:

![image](https://user-images.githubusercontent.com/16706911/68548331-eebb8800-0400-11ea-9c22-6f28922e76ae.png)

this button is shortcut to start the game from scene 1:

![image](_readme/btnFirstScene.jpg)
____________

I usually test my games by changing timescale.

![image](_readme/timescale.jpg)
____________

Also it usefull to test your game with different framerates, to be sure that it is framerate-independent.

![image](_readme/FPS.jpg)
____________

Button to recompile scripts. Usefull when you working on splitting code into .asmdef

![image](_readme/btnRecompile.jpg)
____________

Force reserialize selected(in project window) assets. What it does - https://docs.unity3d.com/ScriptReference/AssetDatabase.ForceReserializeAssets.html

![image](_readme/btnReserializeSelected.jpg)
____________

Force reserialize all assets. Same as previous, but for all assets and takes some time. Use this after adding new asset or updating unity version in order to not spam git history with unwanted changes.

![image](_readme/btnReserializeAll.jpg)
____________
