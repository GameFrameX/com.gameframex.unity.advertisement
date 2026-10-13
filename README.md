<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Advertisement Package

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## Table of Contents

- [Project Overview](#project-overview)
- [Quick Start](#quick-start)
- [Usage Examples](#usage-examples)
- [Documentation & Resources](#documentation--resources)
- [Community & Support](#community--support)
- [License](#license)

---

## Project Overview

GameFrameX Advertisement is an advertisement component for the GameFrameX framework, providing quick integration of advertisement functionality in Unity projects.

### Key Features

- **Easy Integration** - Add advertisement support with minimal setup
- **Component-Based Design** - Built on GameFrameX modular architecture
- **Multiple Ad Networks** - Abstract interface supports various ad SDKs (csj, TopOn, WeChat/Douyin/Kuaishou/Alipay Mini Games, etc.)
- **Reward Ads** - Built-in reward callback handling for incentivized ads
- **Channel Registry** - Channel package Bootstraps self-register; mutually exclusive resolution + explicit channel id override, with zero type-name string reflection

### System Requirements

- Unity 2017.1 or higher

---

## Quick Start

### Installation

Choose one of the following methods:

1. Edit your Unity project's `Packages/manifest.json` and add the `scopedRegistries` section:
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.advertisement": "1.5.0"
     }
   }
   ```

   `scopes` controls which packages are resolved through this registry. Only packages whose names start with `com.gameframex` will be fetched from it.

2. Add to `manifest.json` dependencies:
   ```json
   {
      "com.gameframex.unity.advertisement": "https://github.com/gameframex/com.gameframex.unity.advertisement.git"
   }
   ```
3. Use **Package Manager** in Unity with **Git URL**: `https://github.com/gameframex/com.gameframex.unity.advertisement.git`
4. Clone the repository into your Unity project's `Packages` directory. It will be loaded automatically.
## Usage Examples

### Basic Usage

```csharp
using GameFrameX.Advertisement.Runtime;
using UnityEngine;
using System;

public class MyAdCaller : MonoBehaviour
{
    private AdvertisementComponent adComponent;

    void Start()
    {
        // Get the AdvertisementComponent instance
        adComponent = GetComponent<AdvertisementComponent>();
    }

    // Load ad
    public void LoadAd()
    {
        if (adComponent != null)
        {
            adComponent.Load(
                (message) => { Debug.Log("Ad loaded successfully: " + message); },
                (error) => { Debug.LogError("Ad load failed: " + error); }
            );
        }
    }

    // Show ad
    public void ShowAd()
    {
        if (adComponent != null)
        {
            adComponent.Show(
                (message) => { Debug.Log("Ad shown successfully: " + message); },
                (error) => { Debug.LogError("Ad show failed: " + error); },
                (reward) =>
                {
                    if (reward)
                    {
                        Debug.Log("Ad completed, granting reward");
                    }
                    else
                    {
                        Debug.Log("Ad not fully watched");
                    }
                }
            );
        }
    }
}
```

### Channel Discovery and Configuration (Bootstrap Self-Registration)

The discovery layer is registry-based: each channel package registers its channel factory into `AdvertisementRegistry` via its own `XxxAdvertisementBootstrap` (`RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`); uninstalled channel packages are stripped, so no registration happens (referencing means registering).

- **Mutually exclusive resolution**: `AdvertisementRegistry.ResolveManager()` without arguments picks the first channel in registration order (mini-game channels are naturally mutually exclusive via platform macros);
- **Explicit routing override**: `AdvertisementRegistry.ResolveManager("csj")` explicitly selects a channel, for manual selection when csj / TopOn are both installed;
- **Strongly typed configuration**: channel configuration (appId / adUnitId, etc.) is assigned at startup on each channel Bootstrap's `Config` static property; the factory closure instantiates the channel on first resolution and injects a strongly typed `AdvertisementConfig` subclass;
- **Module wiring**: `AdvertisementComponent`'s module slot is occupied by the main package's default implementation `AdvertisementManager` (channel implementations no longer occupy module slots themselves); all calls and framework shutdown (`IShutdownableModule`) are forwarded to the registry's resolved result.

```csharp
using GameFrameX.Advertisement.Runtime;
using GameFrameX.Advertisement.CSJ.Runtime; // Take the CSJ channel as an example

// Inject channel configuration at startup (before the first ad call)
CSJAdvertisementBootstrap.Config = new CSJAdvertisementConfig
{
    appId = "Your CSJ AppId",
    adUnitId = "Your ad unit ID",
    isDebug = false,
};

// Then use AdvertisementComponent as usual; or explicitly route to a channel
var manager = AdvertisementRegistry.ResolveManager("csj");
```

### Breaking Changes and Migration (v2.1)

- **Inspector type dropdown removed**: `AdvertisementComponent` no longer provides an implementation type dropdown (the original `componentType` type-name string reflection discovery path has been deleted); the discovery layer now converges on `AdvertisementRegistry`;
- **Macro-selected ad unit ID fields removed**: the six serialized fields `Android/iOS/WebGL/WeChat/Douyin/Kuaishou ad unit ID` on the component have been deleted; the ad unit ID has moved into each channel Bootstrap's strongly typed configuration (e.g. `CSJAdvertisementBootstrap.Config.adUnitId`), please move these values into the corresponding channel package's startup configuration;
- **TopOn app configuration moved out of the scene**: the appId / appKey serialized fields on `TopOnAdvertisementBridgeLink` have been deleted and migrated to `TopOnAdvertisementBootstrap.Config` (the BridgeLink only keeps its bridge callback responsibilities); existing projects need to migrate the configuration manually;
- **`AdvertisementConfigAttribute` deleted**: the reflection association between Manager and configuration types (for Inspector reflection drawing) has been removed along with the Inspector reflection chain; registration entries directly carry the factory and configuration.

### Main Classes and Interfaces

- **`AdvertisementComponent`**: Unity MonoBehaviour component, the main entry point for interacting with the ad system (public API and call behavior unchanged).
- **`IAdvertisementManager`**: Core interface for ad managers. All specific ad network implementations must implement this interface.
- **`BaseAdvertisementManager`**: An optional abstract base class implementing `IAdvertisementManager`, providing common callback handling logic.
- **`AdvertisementRegistry`**: Ad channel registry; `Register` / `Unregister` / `Contains` / `GetManager` / `GetAllManagers` / `ResolveManager(channelId = null)` (mutually exclusive resolution + explicit routing override).
- **`AdvertisementChannelRegistration`**: Channel registration descriptor (channel id + display name + `Func<IAdvertisementManager>` factory, with strongly typed configuration injected inside the closure).
- **`AdvertisementManager`**: Main package default implementation; occupies the `IAdvertisementManager` module slot and forwards calls to the registry's resolved result (including `IShutdownableModule` shutdown forwarding).

---

## Documentation & Resources

- **Documentation**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- **Issue Tracker**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## Community & Support

- **QQ Group**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- **Feature Requests**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---


## Dependencies

| Package | Description |
|---------|-------------|
| (无) | - |


## Changelog

See [Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.advertisement/releases) for changelog.
## License

See [LICENSE.md](LICENSE.md) for license information.
