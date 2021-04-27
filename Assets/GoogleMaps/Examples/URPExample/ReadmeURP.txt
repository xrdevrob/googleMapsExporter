This example is designed to work with the Universal Render Pipeline.
It will work with versions of the Universal Rendering Pipeline in
Unity version 2019.3 and later.

It will not work correctly with the standard pipeline.
See Unity URP documentation at:
  https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/manual/index.html

If shader compilation errors occur, the version of the URP pipeline being used
is not compatible with the provided shader. Generally this is due to evolving
Unity package naming conventions prior to Unity version 2019.3

This example requires some manual changes to work correctly. In order to prevent
confusing errors when importing the UnityPackage into a Standard Pipeline
package the Universal Render Pipeline shader is included as a ".txt" file, which
will be ignored by the standard pipeline.

To update this example to a working URP example, the following steps
should be taken:
 - Start a new Universal Render Pipeline project
   (See https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/manual/index.html)
 - Import the GoogleMaps.UnityPackage
 - Outside the Unity Editor, rename
     "Assets/GoogleMaps/Examples/URPExample/Materials/GoogleURPBaseMap.shader.txt"
   to
     "GoogleURPBaseMap.shader"

 - Outside the Unity Editor, rename
     "Assets/GoogleMaps/Examples/URPExample/Materials/GoogleURPBaseMap.shader.txt.meta"
   to
     "GoogleURPBaseMap.shader.meta"

 - NOTE: the Unity environment does not show asset file extensions, so renaming the
   above files cannot be done from within the Unity Editor, and must be done through
   the Windows Explorer, Mac Finder, or equivalent.

 - Open the GoogleMaps/Examples/URPExample/Scenes/URPExample.unity scene
 - In the scene, disable (or delete) the "GameWorld > Instructions > URPOnlyText"
   UI element to remove warning overlay

To exercise more Universal Render Pipeline functionality, and improve visuals
the following steps can be taken to enable post processing effects and improve
shadows:
 - Enable "Post Processing" for "GameWorld > DroneCameraRig > DroneCamera"
 - Select "Global Volume" in the scene root and set the Volume component "Profile"
   field to "GlobalVolumeProfile"
 - Set shadow distance in your Universal Render Pipeline asset to around 1500 meters,
   setting the number of cascades as desired. E.g., for a standard URP project
   configure shadow settings in one or all of:
     UniversalRP-HighQuality, UniversalRP-MediumQuality, UniversalRP-LowQuality
