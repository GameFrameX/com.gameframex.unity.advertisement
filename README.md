# Game Frame X Advertisement
这是一个 Game Frame X 框架的广告组件，用于在 Unity项目中快速集成广告功能。

## 安装指南

1.  打开 Unity 编辑器，选择 `Window` -> `Package Manager`。
2.  在 Package Manager 窗口中，点击 `+` 按钮，然后选择 `Add package from git URL...`。
3.  输入以下 Git URL: `https://github.com/AlianBlank/com.gameframex.unity.advertisement.git` 并点击 `Add`。

本组件支持 Unity 版本 `2017.1` 及更高版本。

## 如何使用

### 1. 添加组件
将 `AdvertisementComponent` 脚本添加到您场景中的任何 GameObject上。

### 2. 配置广告单元 ID
在 Unity 编辑器的 Inspector 窗口中，找到 `Advertisement Component`，然后设置您的广告单元 ID (`Ad Unit Id`)。

### 3. 实现广告管理器
本组件 (`com.gameframex.unity.advertisement`) 提供广告显示的抽象层和核心逻辑。您需要项目中包含一个具体的 `IAdvertisementManager` 接口实现，该实现负责与特定的广告 SDK（例如 AdMob, Unity Ads, IronSource 等）进行交互。

Game Frame X 框架通常允许您在编辑器中或通过代码配置指定使用哪个 `IAdvertisementManager` 实现。请确保您已经设置了相应的广告网络依赖，并注册了您的广告管理器实现。

### 4. 使用方法示例
获取 `AdvertisementComponent` 的引用后，您可以调用其方法来加载和显示广告：

```csharp
using GameFrameX.Advertisement.Runtime;
using UnityEngine;
using System; // 需要引入 System 命名空间以使用 Action

public class MyAdCaller : MonoBehaviour
{
    private AdvertisementComponent adComponent;

    void Start()
    {
        // 获取 AdvertisementComponent 组件实例
        adComponent = GetComponent<AdvertisementComponent>();
        
        // AdvertisementComponent 会在 Start 时自动使用 Inspector 中配置的 AdUnitId 初始化广告管理器
        // 如果需要手动或延迟初始化，或者使用不同的 AdUnitId，可以考虑扩展或修改 AdvertisementComponent
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
                        // 在这里处理奖励逻辑
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

### 回调说明
-   **Load 方法**:
    -   `success` 回调: 当广告成功加载时调用，参数为成功信息 (通常是 JSON 格式的字符串，具体内容取决于广告管理器的实现)。
    -   `fail` 回调: 当广告加载失败时调用，参数为错误信息。
-   **Show 方法**:
    -   `success` 回调: 当广告开始成功展示时调用 (注意，这不一定意味着广告已播放完毕)，参数为成功信息。
    -   `fail` 回调: 当广告展示失败时调用 (例如，没有可展示的广告)，参数为错误信息。
    -   `onShowResult` 回调: 当广告关闭后调用。参数是一个布尔值，`true` 表示广告完整播放（例如，用户观看了整个激励视频），可以发放奖励；`false` 表示广告未完整播放。

## 编辑器集成

-   **AdvertisementComponent Inspector**: `AdvertisementComponent` 提供了一个自定义的 Unity Inspector 界面，您可以在其中方便地设置广告单元 ID (`Ad Unit Id`)。
-   **选择广告管理器实现**: 本组件与 Game Frame X 框架集成。框架通常提供了在编辑器中选择和配置具体 `IAdvertisementManager` 实现的机制。这意味着您可以为不同的广告平台（如 AdMob, Unity Ads 等）创建或使用相应的管理器实现，并通过框架的编辑器工具进行切换和配置，而无需修改核心广告逻辑代码。

## 主要类和接口

-   **`AdvertisementComponent`**: Unity MonoBehaviour 组件，是与广告系统交互的主要入口。您需要将其添加到场景中的 GameObject 上。
-   **`IAdvertisementManager`**: 广告管理器的核心接口。所有具体的广告网络实现（例如 AdMob 管理器、Unity Ads 管理器）都必须实现此接口。它定义了初始化、加载和显示广告的标准方法。
-   **`BaseAdvertisementManager`**: 一个可选的抽象基类，实现了 `IAdvertisementManager` 接口。它可以作为创建自定义广告管理器时的起点，提供了一些通用的回调处理逻辑。

## 其他信息

-   **作者**: Blank (alianblank@outlook.com) - [https://alianblank.com/](https://alianblank.com/)
-   **仓库地址**: [https://github.com/AlianBlank/com.gameframex.unity.advertisement.git](https://github.com/AlianBlank/com.gameframex.unity.advertisement.git)
-   **许可证**: 本项目采用 Apache License 2.0 许可证。详情请参阅 `LICENSE.md` 文件。
