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

## 目次

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

- **簡単統合** - 最小限の設定で広告サポートを追加
- **コンポーネントベース設計** - GameFrameXモジュラーアーキテクチャ基盤
- **複数広告ネットワーク** - 抽象インターフェースで各種広告SDK（csj、TopOn、WeChat/Douyin/Kuaishou/Alipay ミニゲームなど）をサポート
- **リワード広告** - インセンティブ広告の報酬コールバック処理内蔵
- **チャネルレジストリ** - チャネルパッケージの Bootstrap が自己登録、相互排他解決 + 明示的チャネルIDによるオーバーライド、型名文字列リフレクションなし

### システム要件

- Unity 2017.1以上

---

## クイックスタート

### インストール

以下のいずれかの方法を選択してください：

1. Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：
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

   `scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

2. `manifest.json` の `dependencies` に直接追加：
   ```json
   {
      "com.gameframex.unity.advertisement": "https://github.com/gameframex/com.gameframex.unity.advertisement.git"
   }
   ```
3. Unity の **Package Manager** で **Git URL** を使用して追加：`https://github.com/gameframex/com.gameframex.unity.advertisement.git`
4. リポジトリを Unity プロジェクトの `Packages` ディレクトリにクローンしてください。自動的に読み込まれます。
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

### チャネルの発見と設定（Bootstrap 自己登録）

発見層はレジストリベースです：各チャネルパッケージは自身の `XxxAdvertisementBootstrap`（`RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`）を通じてチャネルファクトリを `AdvertisementRegistry` に登録します。未インストールのチャネルパッケージのコードはストリップされるため、登録は発生しません（参照＝登録）。

- **相互排他的解決**：`AdvertisementRegistry.ResolveManager()` を引数なしで呼ぶと登録順で最初のチャネルを取得します（ミニゲームチャネルはプラットフォームマクロにより自然に相互排他）；
- **明示的ルーティングオーバーライド**：`AdvertisementRegistry.ResolveManager("csj")` でチャネルを明示指定。csj / TopOn 併用時の手動選択に使用します；
- **強く型付けされた設定**：チャネル設定（appId / adUnitId など）は起動時に各チャネル Bootstrap の `Config` 静的プロパティへ代入し、ファクトリクロージャが初回解決時にチャネルをインスタンス化して強く型付けされた `AdvertisementConfig` サブクラスを注入します；
- **モジュール接続**：`AdvertisementComponent` のモジュールスロットはメインパッケージのデフォルト実装 `AdvertisementManager` が占めます（チャネル実装はそれぞれモジュールスロットを占有しなくなり）、すべての呼び出しとフレームワークのシャットダウン（`IShutdownableModule`）はレジストリの解決結果へ転送されます。

```csharp
using GameFrameX.Advertisement.Runtime;
using GameFrameX.Advertisement.CSJ.Runtime; // CSJ チャネルの例

// 起動時（最初の広告呼び出し前）にチャネル設定を注入
CSJAdvertisementBootstrap.Config = new CSJAdvertisementConfig
{
    appId = "あなたのCSJ AppId",
    adUnitId = "あなたの広告ユニットID",
    isDebug = false,
};

// その後は通常どおり AdvertisementComponent を使用。またはチャネルへ明示的にルーティング
var manager = AdvertisementRegistry.ResolveManager("csj");
```

### 破壊的変更と移行（v2.1）

- **Inspector のタイプドロップダウン削除**：`AdvertisementComponent` は実装タイプ選択ドロップダウンを提供しなくなりました（旧 `componentType` 型名文字列リフレクションによる発見パスは削除済み）。発見層は `AdvertisementRegistry` に統一されます；
- **マクロ選択の広告ユニットIDフィールド削除**：コンポーネント上の `Android/iOS/WebGL/WeChat/Douyin/Kuaishou 広告ユニットID` の6つのシリアライズフィールドは削除され、広告ユニットIDは各チャネル Bootstrap の強く型付けされた設定へ移行しました（例：`CSJAdvertisementBootstrap.Config.adUnitId`）。これらの値を対応するチャネルパッケージの起動時設定へ移してください；
- **TopOn アプリ設定をシーンから移行**：`TopOnAdvertisementBridgeLink` の appId / appKey シリアライズフィールドは削除され、`TopOnAdvertisementBootstrap.Config` へ移行しました（BridgeLink はブリッジコールバックの責務のみ保持）。既存プロジェクトでは手動での設定移行が必要です；
- **`AdvertisementConfigAttribute` 削除**：Manager と設定型のリフレクション関連付け（Inspector のリフレクション描画用）は Inspector リフレクションチェーンとともに削除され、登録項目がファクトリと設定を直接保持します。

### 主要なクラスとインターフェース

- **`AdvertisementComponent`**: Unity MonoBehaviourコンポーネント、広告システムとのメインエントリポイント（公開APIと呼び出し動作は変更なし）。
- **`IAdvertisementManager`**: 広告マネージャーのコアインターフェース。すべての広告ネットワーク実装はこのインターフェースを実装する必要があります。
- **`BaseAdvertisementManager`**: `IAdvertisementManager`を実装したオプションの抽象基底クラス。共通のコールバック処理ロジックを提供します。
- **`AdvertisementRegistry`**: 広告チャネルレジストリ。`Register` / `Unregister` / `Contains` / `GetManager` / `GetAllManagers` / `ResolveManager(channelId = null)`（相互排他解決 + 明示的ルーティングオーバーライド）。
- **`AdvertisementChannelRegistration`**: チャネル登録記述子（チャネル id + 表示名 + `Func<IAdvertisementManager>` ファクトリ、クロージャ内で強く型付けされた設定を注入）。
- **`AdvertisementManager`**: メインパッケージのデフォルト実装。`IAdvertisementManager` モジュールスロットを占有し、呼び出しをレジストリの解決結果へ転送します（`IShutdownableModule` のシャットダウン転送を含む）。

---

## ドキュメントとリソース

- **ドキュメント**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- **イシュートラッカー**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## コミュニティとサポート

- **QQグループ**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- **機能リクエスト**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---


## 依存関係

| パッケージ | 説明 |
|----------|------|
| (无) | - |


## 変更履歴

[Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.advertisement/releases) で変更履歴を確認してください。
## ライセンス

詳しくは [LICENSE.md](LICENSE.md) をご参照ください。
