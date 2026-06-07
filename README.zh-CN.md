<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Advertisement 广告组件

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · [QQ群](https://qm.qq.com/q/5U9Fvebw)

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>
## 📑 目录导航

- [项目简介](#项目简介)
- [快速开始](#快速开始)
- [使用示例](#使用示例)
- [文档与资源](#文档与资源)
- [社区与支持](#社区与支持)
- [开源协议](#开源协议)

---

## 项目简介

GameFrameX Advertisement 是一个 GameFrameX 框架的广告组件，用于在 Unity 项目中快速集成广告功能。

### 核心特性

- 🎯 **简单集成** - 最少配置即可添加广告支持
- 🔧 **组件化设计** - 基于 GameFrameX 模块化架构
- 📊 **多广告网络** - 抽象接口支持多种广告 SDK（AdMob、Unity Ads、IronSource 等）
- 🎮 **激励广告** - 内置激励视频广告的奖励回调处理
- 🛠️ **编辑器集成** - 自定义 Inspector 方便配置

### 系统要求

- Unity 2017.1 或更高版本

---

## 快速开始

### 安装方式

#### 方式一：Unity Package Manager（推荐）

1. 打开 Unity 编辑器，选择 `Window` -> `Package Manager`
2. 点击 `+` 按钮，选择 `Add package from git URL...`
3. 输入以下 URL：`https://github.com/GameFrameX/com.gameframex.unity.advertisement.git` 并点击 `Add`

### 配置

#### 1. 添加组件

将 `AdvertisementComponent` 脚本添加到场景中的任意 GameObject 上。

#### 2. 配置广告单元 ID

在 Unity 编辑器的 Inspector 窗口中，找到 `Advertisement Component`，设置广告单元 ID（`Ad Unit Id`）。

#### 3. 实现广告管理器

本组件提供广告显示的抽象层和核心逻辑。您需要项目中包含一个具体的 `IAdvertisementManager` 接口实现，该实现负责与特定的广告 SDK（例如 AdMob、Unity Ads、IronSource 等）进行交互。

---

## 使用示例

### 基本使用

```csharp
using GameFrameX.Advertisement.Runtime;
using UnityEngine;
using System;

public class MyAdCaller : MonoBehaviour
{
    private AdvertisementComponent adComponent;

    void Start()
    {
        // 获取 AdvertisementComponent 组件实例
        adComponent = GetComponent<AdvertisementComponent>();
    }

    // 加载广告
    public void LoadAd()
    {
        if (adComponent != null)
        {
            adComponent.Load(
                (message) => { Debug.Log("广告加载成功: " + message); },
                (error) => { Debug.LogError("广告加载失败: " + error); }
            );
        }
    }

    // 显示广告
    public void ShowAd()
    {
        if (adComponent != null)
        {
            adComponent.Show(
                (message) => { Debug.Log("广告展示成功: " + message); },
                (error) => { Debug.LogError("广告展示失败: " + error); },
                (reward) =>
                {
                    if (reward)
                    {
                        Debug.Log("广告播放完成，发放奖励");
                    }
                    else
                    {
                        Debug.Log("广告未完整播放");
                    }
                }
            );
        }
    }
}
```

### 主要类和接口

- **`AdvertisementComponent`**: Unity MonoBehaviour 组件，是与广告系统交互的主要入口。
- **`IAdvertisementManager`**: 广告管理器的核心接口。所有具体的广告网络实现都必须实现此接口。
- **`BaseAdvertisementManager`**: 一个可选的抽象基类，实现了 `IAdvertisementManager` 接口，提供了通用的回调处理逻辑。

---

## 文档与资源

- 📖 **完整文档**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- 🐛 **问题反馈**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## 社区与支持

- 💬 **QQ 讨论群**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- 💡 **功能建议**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---

## 开源协议

本项目采用 **MIT License** 与 **Apache License 2.0** 双许可证分发。

完整许可证文本请参见: [LICENSE.md](LICENSE.md)

---

<div align="center">

**如果这个项目对你有帮助，请给我们一个 ⭐ Star！**

</div>
