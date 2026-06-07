<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Advertisement 廣告組件

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 📑 目錄導航

- [項目簡介](#項目簡介)
- [快速開始](#快速開始)
- [使用範例](#使用範例)
- [文檔與資源](#文檔與資源)
- [社區與支援](#社區與支援)
- [開源協議](#開源協議)

---

## 項目簡介

GameFrameX Advertisement 是一個 GameFrameX 框架的廣告組件，用於在 Unity 項目中快速集成廣告功能。

### 核心特性

- 🎯 **簡單集成** - 最少配置即可添加廣告支持
- 🔧 **組件化設計** - 基於 GameFrameX 模組化架構
- 📊 **多廣告網絡** - 抽象介面支持多種廣告 SDK（AdMob、Unity Ads、IronSource 等）
- 🎮 **激勵廣告** - 內置激勵視頻廣告的獎勵回調處理
- 🛠️ **編輯器集成** - 自定義 Inspector 方便配置

### 系統需求

- Unity 2017.1 或更高版本

---

## 快速開始

### 安裝

編輯 Unity 專案的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：

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
  ]
}
```

`scopes` 控制哪些套件透過此註冊表解析。只有以 `com.gameframex` 開頭的套件才會從這個註冊表取得。

Then add the package to `dependencies`:

```json
{
  "dependencies": {
    "com.gameframex.unity.advertisement": "1.5.0"
  }
}
```

## 使用範例

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
        // 獲取 AdvertisementComponent 組件實例
        adComponent = GetComponent<AdvertisementComponent>();
    }

    // 加載廣告
    public void LoadAd()
    {
        if (adComponent != null)
        {
            adComponent.Load(
                (message) => { Debug.Log("廣告加載成功: " + message); },
                (error) => { Debug.LogError("廣告加載失敗: " + error); }
            );
        }
    }

    // 顯示廣告
    public void ShowAd()
    {
        if (adComponent != null)
        {
            adComponent.Show(
                (message) => { Debug.Log("廣告展示成功: " + message); },
                (error) => { Debug.LogError("廣告展示失敗: " + error); },
                (reward) =>
                {
                    if (reward)
                    {
                        Debug.Log("廣告播放完成，發放獎勵");
                    }
                    else
                    {
                        Debug.Log("廣告未完整播放");
                    }
                }
            );
        }
    }
}
```

### 主要類和介面

- **`AdvertisementComponent`**: Unity MonoBehaviour 組件，是與廣告系統交互的主要入口。
- **`IAdvertisementManager`**: 廣告管理器的核心介面。所有具體的廣告網絡實現都必須實現此介面。
- **`BaseAdvertisementManager`**: 一個可選的抽象基類，實現了 `IAdvertisementManager` 介面，提供了通用的回調處理邏輯。

---

## 文檔與資源

- 📖 **完整文檔**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- 🐛 **問題反饋**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## 社區與支援

- 💬 **QQ 討論群**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- 💡 **功能建議**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---

## 開源協議

詳見 [LICENSE.md](LICENSE.md) 檔案。
