<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Advertisement 廣告組件

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · [QQ群](https://qm.qq.com/q/5U9Fvebw)

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

### 安裝方式

#### 方式一：Unity Package Manager（推薦）

1. 打開 Unity 編輯器，選擇 `Window` -> `Package Manager`
2. 點擊 `+` 按鈕，選擇 `Add package from git URL...`
3. 輸入以下 URL：`https://github.com/GameFrameX/com.gameframex.unity.advertisement.git` 並點擊 `Add`

### 配置

#### 1. 添加組件

將 `AdvertisementComponent` 腳本添加到場景中的任意 GameObject 上。

#### 2. 配置廣告單元 ID

在 Unity 編輯器的 Inspector 視窗中，找到 `Advertisement Component`，設置廣告單元 ID（`Ad Unit Id`）。

#### 3. 實現廣告管理器

本組件提供廣告顯示的抽象層和核心邏輯。您需要項目中包含一個具體的 `IAdvertisementManager` 介面實現，該實現負責與特定的廣告 SDK（例如 AdMob、Unity Ads、IronSource 等）進行交互。

---

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

本項目採用 **MIT License** 與 **Apache License 2.0** 雙許可證分發。

完整許可證文本請參見: [LICENSE.md](LICENSE.md)

---

<div align="center">

**如果這個項目對你有幫助，請給我們一個 ⭐ Star！**

</div>
