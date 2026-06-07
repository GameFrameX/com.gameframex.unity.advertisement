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

Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：

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

`scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

Then add the package to `dependencies`:

```json
{
  "dependencies": {
    "com.gameframex.unity.advertisement": "1.5.0"
  }
}
```

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


## 依存関係

| パッケージ | 説明 |
|----------|------|
| (无) | - |


## 変更履歴

[Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.advertisement/releases) で変更履歴を確認してください。
## ライセンス

詳しくは [LICENSE.md](LICENSE.md) をご参照ください。
