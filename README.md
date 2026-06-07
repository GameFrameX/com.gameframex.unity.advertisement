<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Advertisement Package

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>
## 📑 Table of Contents

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

- 🎯 **Easy Integration** - Add advertisement support with minimal setup
- 🔧 **Component-Based Design** - Built on GameFrameX modular architecture
- 📊 **Multiple Ad Networks** - Abstract interface supports various ad SDKs (AdMob, Unity Ads, IronSource, etc.)
- 🎮 **Reward Ads** - Built-in reward callback handling for incentivized ads
- 🛠️ **Editor Integration** - Custom Inspector for easy configuration

### System Requirements

- Unity 2017.1 or higher

---

## Quick Start

### Installation

#### Method 1: Unity Package Manager (Recommended)

1. Open Unity Editor, go to `Window` -> `Package Manager`
2. Click the `+` button and select `Add package from git URL...`
3. Enter the following URL: `https://github.com/GameFrameX/com.gameframex.unity.advertisement.git` and click `Add`

### Configuration

#### 1. Add Component

Add the `AdvertisementComponent` script to any GameObject in your scene.

#### 2. Configure Ad Unit ID

In the Unity Inspector window, find the `Advertisement Component` and set your Ad Unit ID (`Ad Unit Id`).

#### 3. Implement Ad Manager

This component provides the abstraction layer and core logic for ad display. You need a concrete `IAdvertisementManager` interface implementation in your project that interacts with a specific ad SDK (e.g., AdMob, Unity Ads, IronSource, etc.).

---

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

### Main Classes and Interfaces

- **`AdvertisementComponent`**: Unity MonoBehaviour component, the main entry point for interacting with the ad system.
- **`IAdvertisementManager`**: Core interface for ad managers. All specific ad network implementations must implement this interface.
- **`BaseAdvertisementManager`**: An optional abstract base class implementing `IAdvertisementManager`, providing common callback handling logic.

---

## Documentation & Resources

- 📖 **Documentation**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- 🐛 **Issue Tracker**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## Community & Support

- 💬 **QQ Group**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- 💡 **Feature Requests**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---

## License

This project is distributed under **MIT License** and **Apache License 2.0** dual licensing.

See full license text: [LICENSE.md](LICENSE.md)

---

<div align="center">

**If this project helps you, please give us a ⭐ Star!**

</div>
