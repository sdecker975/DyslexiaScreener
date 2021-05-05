QualitySettingsMenu

Version :

 Unity 5.6 : (and up)
   1.2   removed duplicate resolution option buttons in the drop down menu. 

 Unity 5.5 : (The Unity 5.5 version does not work on Unity 5.4)
   1.1   small fixes.
   1.0   added Shadow Resolution and Texture Quality sliders. (added a sample menu of my "Royal UI" package available on the asset store)
   0.5.3 small adjustments/finetuning 
         removed asigning camera to canvas set to "ScreenSpace-Camera".

 Unity 5.4 :
   1.1   small fixes.
   1.0   added Shadow Resolution and Texture Quality sliders. (added a sample menu of my "Royal UI" package available on the asset store)
   0.5.3 small adjustments/finetuning 
         removed asigning camera to canvas set to "ScreenSpace-Camera".

 Unity 5 through 5.3 :
   0.5.2
   0.5.1 added a "Toon" version of the menu.
   0.5   small fixes/optimizations.
   0.4   added option to save to ini file.
   0.3   made resolution menu scrollable.
   0.2   fixed asigning camera to canvas when loading new scene.
   0.1   initial release.
      
All you need to do is drag the "_QualitySettingsMenu" prefab into the first scene of your Game/Project.
!!Please make sure you have a "EventSystem" added to every scene or the canvas will not work.

(Adding the prefab to a later scene is fine too, but be aware the menu is set to "DontDestroyOnLoad()".
Reloading a scene/level that starts with the menu already in it will result in multiple menu prefabs in that scene)

Select how you want to save (playerPrefs or .ini file). 
Saving to .ini file will create a text file in your build projects main folder called "QualitySettings.ini".
(When running in the unity editor it will save to the main asset folder of your project).

If you want to pause time when the menu is open set the "Pause Time when menu open" bool on the SettingsMenu script in the inspector to "True".

To open the menu in game press Escape (but you can change this to any Key/Button you want).
Or you can use a UI button with the "OpenQualitySettingMenu()" function as an OnClickevent.

The menu lets you switch between the Quality Levels and lets you set the AA, Vcync and AF seperately,
Change resolution and toggle fullScreen/Windowed mode.
The Unity standard FPS counter asset is build in.
And a Vsync toggle.

Pressing the "QuitGame" button or closing the menu will save your setting.
(keep this in mind when you want to delete playerprefs.)

The first time the game runs, the way the menu is set will determen the default Quality setting, so set these the way you want them to be before building.

I recomend disabling/hiding the "Display Resolution Dialog" menu in the Player Settings, for these settings will be ignored.

there is a "Delete Playerprefs" button added but disabled, this may be usefull when testing settings.

When run in the Game View window the resolution dropbox will not show you much options,
but when build, this will be filled with all your monitors available resolutions.

Use the "Layout Element" script min Hight on the button prefab to change it`s sice (when using you own UI/sprite art).
The buttons are layed out according to these settings.
Do not change the scale of the button prefab itself this wil have unwanted results.

-If you like the asset, rating it on the Asset store would be greatly appreciated.-
-If you don`t like the asset letting me know why would be equally helpful.-

for any questions, comments or suggestions feel free to contact me.


The RoyalUI menu is made with a set of UI buttons and panels from one of my other packages on the AssetStore.
"Royal UI".