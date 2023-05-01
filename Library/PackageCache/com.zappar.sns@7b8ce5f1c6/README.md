# WebGL Save and Share unity package

This package allows you to easily implement a snapshot or video save/share functionality into your WebGL applications. It includes the ability to save the result and, where available, the native Web Share API makes it possible to social share to other apps installed on the device.​

## Importing SNS package into Unity

If you're using [Universal AR (UAR) Unity SDK](https://github.com/zappar-xr/universal-ar-unity) version 3.1.0 or above, you can directly add this package from Zappar menu option `Zappar > Additional Packages > Add\Update WebGL Save And Share` (you may have to change focus of Editor Window or restart Unity after this if you see any error). Otherwise, you can import the package from the Unity editor by following these steps:
1. Opening the `Package Manager` from `Window > Package Manager` from Editor
2. Locate the `+` button on the top left corner and select `Add package from git URL...`
3. Enter the following URL: `https://github.com/zappar-xr/unity-webgl-sns.git`

This will automatically fetch the latest version of the package from Github.

Another option is to add the `WebGL Save And Share` package in your projects' `manifest.json` file located under `Root_Directory>Packages`.

```
{
  "dependencies": {
    "com.zappar.sns": "https://github.com/zappar-xr/unity-webgl-sns.git"
  }
}
```

Note that you can modify the source Github URL to define any particular `tag`, `branch` or `commit hash`, by adding a suffix: `#ID` to the git URL. You can read more about it here: https://docs.unity3d.com/Manual/upm-git.html

## Platform support

This library is currently supported only on Unity WebGL with the Unity version of `2021.x LTS` and above. There are placeholder APIs for editor mode development, but you would always need to build the project for testing any functionality.


## Usage

`ZSaveNShare.cs` is the main script that exposes all the required APIs and callbacks you need to use the package. The typical flow of operation is as follows:
1. Call the `ZSaveNShare.Initialize()` at the start of the scene. Normally you would call this from MonoBehaviours' `Awake` or `Start` method which will set up the library for use.
2. Next you would want to subscribe for events from the native plugin. The `ZSaveNShare.RegisterSNSCallbacks(...)` allows you to register your callback methods for three main action prompts - `OnSaved`, `OnShared`, and `OnClosed`.
3. Call `ZSaveNShare.DeregisterSNSCallbacks(...)` to unsubscribe for the plugin events at the end.
4. Before calling to open the prompt you would need to set the data. Use `ZSaveNShare.TakeSnapshot(...)`, which is a C# coroutine to save the current unity frame. This internally calls `zappar_sns_jpg_snapshot` to set the data to be used with the prompt.
5. `ZSaveNShare.OpenSNSSnapPrompt()` opens the prompt for the user to either save or share the media.
6. Remember to update your webgl template or final index.html to define `uarGameInstance`. Read [Updates to WebGL Template](#updates-to-webgl-template) section for details.

The package includes an example scene to demonstrate the usage of the plugin. Look for the `ZSNSTest.cs` script to understand the flow.

You can also use this package along with [@Zappar/video-recorder](https://www.npmjs.com/package/@zappar/video-recorder) package,find the `VideoRecorder` Unity package [here](https://github.com/zappar-xr/unity-webgl-video-recorder/blob/main/README.md).


## Updates to WebGL Template

Lastly, to allow the plugin to send messages and events to the Unity game instance you would need to define uarGameInstance in the global window scope. Add the following line in the promise resolution state of the **createUnityInstance** method

`window.uarGameInstance=unityInstance;`

For example, in the default Unity WebGL template find the section where createUnityInstance method is called, and add this line inside the `.then((unityInstance)=> { ... })` block as follows:

```
createUnityInstance(canvas, config, (progress) => {
          progressBarFull.style.width = 100 * progress + "%";
        }).then((unityInstance) => {
          loadingBar.style.display = "none";
          window.uarGameInstance=unityInstance;
          fullscreenButton.onclick = () => {
            unityInstance.SetFullscreen(1);
          };
        }).catch((message) => {
          alert(message);
        });
```

You can define this inside your own WebGL Template or in the final `index.html` after the build.

## Caveats

The user flow for this package's save and share options will differ depending on the device. This is because the package uses the browser’s default Web Share API handler.

### Android

Android users will see both the Save and Share button side by side.

### iOS

iOS 14 and older users must first Save their image and then click on the Open Files App button in order to open their device's 'Files' application. They will then be able to share their snapshot from their native device.

iOS 15 makes this much easier and allows for sharing directly in the user flow.