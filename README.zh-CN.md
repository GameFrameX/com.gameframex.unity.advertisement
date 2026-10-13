<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Advertisement 广告组件

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 目录导航

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

- **简单集成** - 最少配置即可添加广告支持
- **组件化设计** - 基于 GameFrameX 模块化架构
- **多广告网络** - 抽象接口支持多种广告 SDK（csj、TopOn、微信/抖音/快手/支付宝小游戏等）
- **激励广告** - 内置激励视频广告的奖励回调处理
- **渠道注册表** - 渠道包 Bootstrap 自注册，互斥解析 + 显式渠道标识覆盖，零类型名字符串反射

### 系统要求

- Unity 2017.1 或更高版本

---

## 快速开始

### 安装

选择以下任一方式：

1. 编辑 Unity 项目的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
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

   `scopes` 控制哪些包通过此注册表解析。只有以 `com.gameframex` 开头的包才会从这个注册表获取。

2. 直接在 `manifest.json` 的 `dependencies` 节点下添加以下内容：
   ```json
   {
      "com.gameframex.unity.advertisement": "https://github.com/gameframex/com.gameframex.unity.advertisement.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加库，地址为：`https://github.com/gameframex/com.gameframex.unity.advertisement.git`
4. 直接下载仓库放置到 Unity 项目的 `Packages` 目录下，会自动加载识别。
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


### 渠道发现与配置（Bootstrap 自注册）

渠道发现层基于注册表：各渠道包通过自身的 `XxxAdvertisementBootstrap`（`RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`）把渠道工厂注册进 `AdvertisementRegistry`；未安装的渠道包代码被裁剪，注册不会发生（引用即注册）。

- **互斥解析**：`AdvertisementRegistry.ResolveManager()` 不带参数时按注册顺序取首个渠道（小游戏渠道由平台宏保证天然互斥）；
- **显式路由覆盖**：`AdvertisementRegistry.ResolveManager("csj")` 显式指定渠道，用于 csj / TopOn 同装时的人工选型；
- **强类型配置**：渠道配置（appId / adUnitId 等）在各渠道 Bootstrap 的 `Config` 静态属性上于启动期赋值，工厂闭包在首次解析时实例化渠道并注入强类型 `AdvertisementConfig` 子类；
- **模块接线**：`AdvertisementComponent` 的模块槽位由主包默认实现 `AdvertisementManager` 占据（渠道实现不再各自占用模块槽位），所有调用与框架关闭（`IShutdownableModule`）都转发给注册表解析结果。

```csharp
using GameFrameX.Advertisement.Runtime;
using GameFrameX.Advertisement.CSJ.Runtime; // 以穿山甲渠道为例

// 启动期（首次广告调用前）注入渠道配置
CSJAdvertisementBootstrap.Config = new CSJAdvertisementConfig
{
    appId = "你的穿山甲 AppId",
    adUnitId = "你的广告位ID",
    isDebug = false,
};

// 之后照常使用 AdvertisementComponent；或显式路由到指定渠道
var manager = AdvertisementRegistry.ResolveManager("csj");
```

### 破坏性变更与迁移（v2.1）

- **移除 Inspector 类型下拉**：`AdvertisementComponent` 不再提供实现类型选择下拉（原 `componentType` 类型名字符串反射发现路径已删除），发现层统一收敛到 `AdvertisementRegistry`；
- **移除宏选广告位ID字段**：组件上的 `Android/iOS/WebGL/微信/抖音/快手 广告位ID` 六个序列化字段已删除，广告位ID 下沉到各渠道 Bootstrap 的强类型配置（如 `CSJAdvertisementBootstrap.Config.adUnitId`），请把这些值搬到对应渠道包的启动期配置；
- **TopOn 应用配置迁出场景**：`TopOnAdvertisementBridgeLink` 上的 appId / appKey 序列化字段已删除，迁移到 `TopOnAdvertisementBootstrap.Config`（BridgeLink 仅保留桥接回调职责），既有工程需手动搬移配置；
- **`AdvertisementConfigAttribute` 已删除**：Manager 与配置类型的反射关联（供 Inspector 反射绘制）随 Inspector 反射链一并移除，注册项直接携带工厂与配置。
### 主要类和接口

- **`AdvertisementComponent`**: Unity MonoBehaviour 组件，是与广告系统交互的主要入口（公开 API 与调用行为不变）。
- **`IAdvertisementManager`**: 广告管理器的核心接口。所有具体的广告网络实现都必须实现此接口。
- **`BaseAdvertisementManager`**: 一个可选的抽象基类，实现了 `IAdvertisementManager` 接口，提供了通用的回调处理逻辑。
- **`AdvertisementRegistry`**: 广告渠道注册表；`Register` / `Unregister` / `Contains` / `GetManager` / `GetAllManagers` / `ResolveManager(channelId = null)`（互斥解析 + 显式路由覆盖）。
- **`AdvertisementChannelRegistration`**: 渠道注册描述（渠道 id + 展示名 + `Func<IAdvertisementManager>` 工厂，闭包内注入强类型配置）。
- **`AdvertisementManager`**: 主包默认实现，占据 `IAdvertisementManager` 模块槽位并把调用转发给注册表解析结果（含 `IShutdownableModule` 关闭转发）。

---

## 文档与资源

- **完整文档**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- **问题反馈**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## 社区与支持

- **QQ 讨论群**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- **功能建议**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---


## 依赖

| 包 | 说明 |
|----|------|
| (无) | - |


## 更新日志

查看 [Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.advertisement/releases) 了解更新日志。
## 开源协议

详见 [LICENSE.md](LICENSE.md) 文件。
