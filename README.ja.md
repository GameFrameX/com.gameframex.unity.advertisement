<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Advertisement パッケージ

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>
## 📑 目次

- [プロジェクト概要](#プロジェクト概要)
- [クイックスタート](#クイックスタート)
- [使用例](#使用例)
- [ドキュメントとリソース](#ドキュメントとリソース)
- [コミュニティとサポート](#コミュニティとサポート)
- [ライセンス](#ライセンス)

---

## プロジェクト概要

GameFrameX Advertisementは、GameFrameXフレームワークの広告コンポーネントで、Unityプロジェクトに広告機能を素早く統合できます。

### 主な機能

- 🎯 **簡単統合** - 最小限の設定で広告サポートを追加
- 🔧 **コンポーネントベース設計** - GameFrameXモジュラーアーキテクチャ基盤
- 📊 **複数広告ネットワーク** - 抽象インターフェースで各種広告SDK（AdMob、Unity Ads、IronSourceなど）をサポート
- 🎮 **リワード広告** - インセンティブ広告の報酬コールバック処理内蔵
- 🛠️ **エディタ統合** - カスタムインスペクタで簡単設定

### システム要件

- Unity 2017.1以上

---

## クイックスタート

### インストール

#### 方法1: Unity Package Manager（推奨）

1. Unityエディタを開き、`Window` -> `Package Manager`に移動
2. `+`ボタンをクリックし、`Add package from git URL...`を選択
3. 次のURLを入力：`https://github.com/GameFrameX/com.gameframex.unity.advertisement.git` そして`Add`をクリック

### 設定

#### 1. コンポーネントの追加

シーン内のGameObjectに`AdvertisementComponent`スクリプトを追加します。

#### 2. 広告ユニットIDの設定

Unityインスペクタウィンドウで`Advertisement Component`を見つけ、広告ユニットID（`Ad Unit Id`）を設定します。

#### 3. 広告マネージャーの実装

このコンポーネントは広告表示の抽象レイヤーとコアロジックを提供します。特定の広告SDK（AdMob、Unity Ads、IronSourceなど）と対話する具体的な`IAdvertisementManager`インターフェース実装が必要です。

---

## 使用例

### 基本的な使用方法

```csharp
using GameFrameX.Advertisement.Runtime;
using UnityEngine;
using System;

public class MyAdCaller : MonoBehaviour
{
    private AdvertisementComponent adComponent;

    void Start()
    {
        // AdvertisementComponentインスタンスを取得
        adComponent = GetComponent<AdvertisementComponent>();
    }

    // 広告の読み込み
    public void LoadAd()
    {
        if (adComponent != null)
        {
            adComponent.Load(
                (message) => { Debug.Log("広告の読み込み成功: " + message); },
                (error) => { Debug.LogError("広告の読み込み失敗: " + error); }
            );
        }
    }

    // 広告の表示
    public void ShowAd()
    {
        if (adComponent != null)
        {
            adComponent.Show(
                (message) => { Debug.Log("広告の表示成功: " + message); },
                (error) => { Debug.LogError("広告の表示失敗: " + error); },
                (reward) =>
                {
                    if (reward)
                    {
                        Debug.Log("広告視聴完了、報酬を付与");
                    }
                    else
                    {
                        Debug.Log("広告を最後まで視聴していません");
                    }
                }
            );
        }
    }
}
```

### 主要なクラスとインターフェース

- **`AdvertisementComponent`**: Unity MonoBehaviourコンポーネント、広告システムとのメインエントリポイント。
- **`IAdvertisementManager`**: 広告マネージャーのコアインターフェース。すべての広告ネットワーク実装はこのインターフェースを実装する必要があります。
- **`BaseAdvertisementManager`**: `IAdvertisementManager`を実装したオプションの抽象基底クラス。共通のコールバック処理ロジックを提供します。

---

## ドキュメントとリソース

- 📖 **ドキュメント**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- 🐛 **イシュートラッカー**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## コミュニティとサポート

- 💬 **QQグループ**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- 💡 **機能リクエスト**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---

## ライセンス

このプロジェクトは**MIT License**と**Apache License 2.0**の二重ライセンスで配布されています。

完全なライセンステキスト: [LICENSE.md](LICENSE.md)

---

<div align="center">

**このプロジェクトが役立ったら、⭐ をください！**

</div>
