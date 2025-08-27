<p align="center" ><img src="http://oi60.tinypic.com/116jd51.jpg" alt="Particle" title="Particle"></p>

# Particle Device Setup library *(Alpha)*

[![license](https://img.shields.io/hexpm/l/plug.svg)](https://github.com/spark/particle-windows-devicesetup/blob/master/LICENSE)
[![NuGet Version](http://img.shields.io/nuget/v/Particle.Setup.svg?style=flat)](https://www.nuget.org/packages/Particle.Setup/)

## Introduction

The Particle Device Setup library *(Alpha)* is meant for integrating the initial setup process of Particle devices in your app. This library will enable you to easily invoke a standalone setup wizard UI for setting up internet-connected products powered by a Particle device (Photon, P0, P1). The setup UI can be easily customized by a customization proxy class, that includes: look & feel, colors, texts and fonts as well as custom brand logos and custom instructional video for your product. There are good defaults in place if you don't set these properties, but you can override the look and feel as needed to suit the rest of your app.

The wireless setup process for the Photon uses very different underlying technology from the Core. Where the Core used TI SmartConfig, the Photon uses what we call "soft AP" - i.e.: the Photon advertises a Wi-Fi network, you join that network from your mobile app to exchange credentials, and then the Photon connects using the Wi-Fi credentials you supplied.

With the Device Setup library, you make one simple call from your app, for example when the user hits a "Setup my device" button, and a whole series of screens then guide the user through the setup process. When the process finishes, the app user is back on the screen where she hit the "Setup my device" button, and your code has been passed an instance of the device she just setup and claimed. Windows Device setup library is implemented as an open-source .NET Portable Class Library. It works well for both C# and VB projects.

## Alpha notice

This Library is still under development and is currently released as Alpha and over the next few months may go under considerable changes. Although tested, bugs and issues may be present. Some code might require cleanup. In addition, until version 1.0 is released, we cannot guarantee that API calls will not break from one version to the next. Be sure to consult the [Change Log](https://github.com/spark/particle-windows-devicesetup/blob/master/CHANGELOG.md) for any breaking changes / additions to the library.

## Getting started

- Perform the installation step described under the [Installation](#installation) section below for integrating in your own project
- Be sure to check [Usage](#usage) before you begin for some code examples

## Usage

Due to limitations on how string resources are handled in a PCL (Portable Class Library) you will need to inject the resources from the setup library into your app:

You can either do this on app startup, or before you start the soft AP process.

```cs
UI.WindowsRuntimeResourceManager.InjectIntoResxGeneratedApplicationResourcesClass(
  typeof(Particle.Setup.SetupResources)
);
```

You will need to create an instance of the `SoftAPSettings` class to pass to `SoftAP.Start()`:

And populate the following required properties:

| Value | Meaning |
| --- | --- |
| `AppFrame` | The main application frame |
| `CompletionPageType` | The page to show after completion |
| `Username` | The username of the user if you want it shown |
| `CurrentDeviceNames` | A HashSet of current device names to check against when assigning a name |
| `OnSoftAPExit` | A function of type `SoftAPExitEventHandler` that is called when the soft AP process exits |

If you are developing an application for Windows Phone, you will need to handle the hardware back buttons.

Define BackPressed EventHandler:

```cs
void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
{
  e.Handled = SoftAP.BackButtonPressed();
}
```

Add the handler:

```cs
HardwareButtons.BackPressed += HardwareButtons_BackPressed;
```

And set it to remove on soft AP exit (and make sure to add this to your `SoftAPSettings.OnSoftAPExit`:

```cs
void SoftAPSettings_OnSoftAPExit()
{
  HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
}
```

And finally you can call start:
```cs
SoftAP.Start(softAPSettings);
```

When the soft AP process is complete, the static class `SoftAP.SoftAPResult` will be populated with the results

`Result`: The result of the process
`ParticleDevice`: The particle device that just went through the soft AP process

Possible result values:

| Value | Meaning |
| --- | --- |
| `Success` | The process completed successfully |
| `SuccessUnknown` | The process completed successfully, but you do not own the device |
| `SuccessDeviceOffline` | The process completed successfully, but is currently offline |
| `FailureConfigure` | The device was unable to be configured |

## Internally used direct soft AP commands

The Device Setup library currently also has the internally used class: [`SoftAPConfig`](https://github.com/spark/particle-windows-devicesetup/blob/master/SoftAPConfig.cs) which is an object that enables all basic soft AP commands.

### Get device ID

```cs
var deviceID = await SoftAPConfig.GetDeviceIdAsync();
```

### Get public key

```cs
var publicKey = await SoftAPConfig.GetPublicKeyAsync();
```

### Get scan AP's

```cs
var scanAPs = await SoftAPConfig.GetScanAPsAsync();
```

### Get version

```cs
var version = await SoftAPConfig.GetVersionAsync();
```

### Set claim code

* claimCode is the result from a call from `ParticleCloud.SharedCloud.CreateClaimCodeAsync()`

```cs
var result = await SoftAPConfig.SetClaimCodeAsync(claimCode);
```

### Set configure AP

* index is the index you want to use for your call to `SetConnectAPAsync`
* scanAP is a scanAP from a call from `GetScanAPsAsync`
* password is the plain text password for the above scanAP
* publicKey is the publicKey from a call from `GetPublicKeyAsync`

```cs
var result = await SoftAPConfig.SetConfigureAPAsync(index, scanAP, password, publicKey);
```

### Set connect AP

* index is the index you used in your call to `SetConfigureAPAsync`

```cs
var result = await SoftAPConfig.SetConnectAPAsync(index);
```

## Installation

- There is currently one version of the library:
 - **portable46-win81+wpa81**: Portable .NET Framework 4.6 for use in Windows Runtime (WinRT) applications (Windows 8.1+ and Windows Phone 8.1+)
- Any edition of Microsoft Visual Studio 2015 (Other build systems may also work, but are not officially supported.)
- You can use either C# or VB

You can either [download Particle Device Setup library](https://github.com/spark/particle-windows-devicesetup/archive/master.zip) or install using [NuGet](http://www.nuget.org/packages/Particle.Setup)

`PM> Install-Package Particle.Setup`

## Communication

- If you **need help**, use [Our community website](http://community.particle.io), use the `Mobile` category for discussion/troubleshooting Windows apps using the Particle Windows Cloud SDK.

- If you are certain you **found a bug**, _and can provide steps to reliably reproduce it_, [open an issue on GitHub](https://github.com/spark/particle-windows-devicesetup/labels/bug).
- If you **have a feature request**, [open an issue on GitHub](https://github.com/spark/particle-windows-devicesetup/labels/enhancement).
- If you **want to contribute**, submit a pull request, be sure to check out spark.github.io for our contribution guidelines, and please sign the [CLA](https://part.cl/icla).

## Maintainers

- Justin Myers [Github](https://github.com/justmobilize)
- Ido Kleinman [Github](https://www.github.com/idokleinman) | [Twitter](https://www.twitter.com/idokleinman)

## License

Particle Device Setup library is available under the Apache License 2.0. See the [LICENSE file](https://github.com/spark/particle-windows-devicesetup/blob/master/LICENSE) for more info.
