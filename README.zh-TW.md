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

## 目錄導航

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

- **簡單集成** - 最少配置即可添加廣告支持
- **組件化設計** - 基於 GameFrameX 模組化架構
- **多廣告網絡** - 抽象介面支持多種廣告 SDK（csj、TopOn、微信/抖音/快手/支付寶小遊戲等）
- **激勵廣告** - 內置激勵視頻廣告的獎勵回調處理
- **渠道註冊表** - 渠道包 Bootstrap 自註冊，互斥解析 + 顯式渠道標識覆蓋，零類型名字串反射

### 系統需求

- Unity 2017.1 或更高版本

---

## 快速開始

### 安裝

選擇以下任一方式：

1. 編輯 Unity 專案的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
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

   `scopes` 控制哪些套件透過此註冊表解析。只有以 `com.gameframex` 開頭的套件才會從這個註冊表取得。

2. 直接在 `manifest.json` 的 `dependencies` 節點下添加以下內容：
   ```json
   {
      "com.gameframex.unity.advertisement": "https://github.com/gameframex/com.gameframex.unity.advertisement.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加庫，地址為：`https://github.com/gameframex/com.gameframex.unity.advertisement.git`
4. 直接下載倉庫放置到 Unity 專案的 `Packages` 目錄下，會自動載入識別。
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

### 渠道發現與配置（Bootstrap 自註冊）

渠道發現層基於註冊表：各渠道包通過自身的 `XxxAdvertisementBootstrap`（`RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`）把渠道工廠註冊進 `AdvertisementRegistry`；未安裝的渠道包代碼被裁剪，註冊不會發生（引用即註冊）。

- **互斥解析**：`AdvertisementRegistry.ResolveManager()` 不帶參數時按註冊順序取首個渠道（小遊戲渠道由平台宏保證天然互斥）；
- **顯式路由覆蓋**：`AdvertisementRegistry.ResolveManager("csj")` 顯式指定渠道，用於 csj / TopOn 同裝時的人工選型；
- **強型別配置**：渠道配置（appId / adUnitId 等）在各渠道 Bootstrap 的 `Config` 靜態屬性上於啟動期賦值，工廠閉包在首次解析時實例化渠道並注入強型別 `AdvertisementConfig` 子類；
- **模組接線**：`AdvertisementComponent` 的模組槽位由主包默認實現 `AdvertisementManager` 佔據（渠道實現不再各自佔用模組槽位），所有調用與框架關閉（`IShutdownableModule`）都轉發給註冊表解析結果。

```csharp
using GameFrameX.Advertisement.Runtime;
using GameFrameX.Advertisement.CSJ.Runtime; // 以穿山甲渠道為例

// 啟動期（首次廣告調用前）注入渠道配置
CSJAdvertisementBootstrap.Config = new CSJAdvertisementConfig
{
    appId = "你的穿山甲 AppId",
    adUnitId = "你的廣告位ID",
    isDebug = false,
};

// 之後照常使用 AdvertisementComponent；或顯式路由到指定渠道
var manager = AdvertisementRegistry.ResolveManager("csj");
```

### 破壞性變更與遷移（v2.1）

- **移除 Inspector 類型下拉**：`AdvertisementComponent` 不再提供實現類型選擇下拉（原 `componentType` 類型名字串反射發現路徑已刪除），發現層統一收斂到 `AdvertisementRegistry`；
- **移除宏選廣告位ID欄位**：組件上的 `Android/iOS/WebGL/微信/抖音/快手 廣告位ID` 六個序列化欄位已刪除，廣告位ID 下沉到各渠道 Bootstrap 的強型別配置（如 `CSJAdvertisementBootstrap.Config.adUnitId`），請把這些值搬到對應渠道包的啟動期配置；
- **TopOn 應用配置遷出場景**：`TopOnAdvertisementBridgeLink` 上的 appId / appKey 序列化欄位已刪除，遷移到 `TopOnAdvertisementBootstrap.Config`（BridgeLink 僅保留橋接回調職責），既有工程需手動搬移配置；
- **`AdvertisementConfigAttribute` 已刪除**：Manager 與配置類型的反射關聯（供 Inspector 反射繪製）隨 Inspector 反射鏈一併移除，註冊項直接攜帶工廠與配置。

### 主要類和介面

- **`AdvertisementComponent`**: Unity MonoBehaviour 組件，是與廣告系統交互的主要入口（公開 API 與調用行為不變）。
- **`IAdvertisementManager`**: 廣告管理器的核心介面。所有具體的廣告網絡實現都必須實現此介面。
- **`BaseAdvertisementManager`**: 一個可選的抽象基類，實現了 `IAdvertisementManager` 介面，提供了通用的回調處理邏輯。
- **`AdvertisementRegistry`**: 廣告渠道註冊表；`Register` / `Unregister` / `Contains` / `GetManager` / `GetAllManagers` / `ResolveManager(channelId = null)`（互斥解析 + 顯式路由覆蓋）。
- **`AdvertisementChannelRegistration`**: 渠道註冊描述（渠道 id + 展示名 + `Func<IAdvertisementManager>` 工廠，閉包內注入強型別配置）。
- **`AdvertisementManager`**: 主包默認實現，佔據 `IAdvertisementManager` 模組槽位並把調用轉發給註冊表解析結果（含 `IShutdownableModule` 關閉轉發）。

---

## 文檔與資源

- **完整文檔**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- **問題反饋**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## 社區與支援

- **QQ 討論群**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- **功能建議**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---


## 依賴

| 套件 | 說明 |
|------|------|
| (无) | - |


## 更新日誌

查看 [Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.advertisement/releases) 了解更新日誌。
## 開源協議

詳見 [LICENSE.md](LICENSE.md) 檔案。
