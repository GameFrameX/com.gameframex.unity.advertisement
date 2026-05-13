// ==========================================================================================
//   GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//   GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//   均受中华人民共和国及相关国际法律法规保护。
//   are protected by the laws of the People's Republic of China and relevant international regulations.
//   使用本项目须严格遵守相应法律法规及开源许可证之规定。
//   Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//   本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//   This project is dual-licensed under the MIT License and Apache License 2.0,
//   完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//   please refer to the LICENSE file in the root directory of the source code for the full license text.
//   禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//   It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//   侵犯他人合法权益等法律法规所禁止的行为！
//   or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//   因基于本项目二次开发所产生的一切法律纠纷与责任，
//   Any legal disputes and liabilities arising from secondary development based on this project
//   本项目组织与贡献者概不承担。
//   shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//   GitHub 仓库：https://github.com/GameFrameX
//   GitHub Repository: https://github.com/GameFrameX
//   Gitee  仓库：https://gitee.com/GameFrameX
//   Gitee Repository:  https://gitee.com/GameFrameX
//   CNB  仓库：https://cnb.cool/GameFrameX
//   CNB Repository:  https://cnb.cool/GameFrameX
//   官方文档：https://gameframex.doc.alianblank.com/
//   Official Documentation: https://gameframex.doc.alianblank.com/
//  ==========================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using GameFrameX.Runtime;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告管理器抽象基类，提供新旧 API 的双向委托及通用回调管理。
    /// </summary>
    /// <remarks>
    /// Abstract base class for advertisement managers, providing bidirectional delegation between legacy and new APIs and common callback management.
    /// </remarks>
    [Preserve]
    public abstract class BaseAdvertisementManager : GameFrameworkModule, IAdvertisementManager
    {
        private bool _delegating;

        /// <summary>
        /// 更新方法，每帧由框架调用。
        /// </summary>
        /// <remarks>
        /// Update method called by the framework every frame.
        /// </remarks>
        /// <param name="elapseSeconds">逻辑流逝时间（秒） / Elapsed logic time in seconds</param>
        /// <param name="realElapseSeconds">真实流逝时间（秒） / Elapsed real time in seconds</param>
        protected override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭时的清理方法。
        /// </summary>
        /// <remarks>
        /// Cleanup method invoked on shutdown.
        /// </remarks>
        protected override void Shutdown()
        {
        }

        /// <summary>
        /// 广告展示结果回调。
        /// </summary>
        /// <remarks>
        /// Callback for advertisement show result.
        /// </remarks>
        [Preserve] protected Action<bool> OnShowResult;

        /// <summary>
        /// 广告加载成功回调。
        /// </summary>
        /// <remarks>
        /// Callback for advertisement load success.
        /// </remarks>
        [Preserve] protected Action<string> OnLoadSuccess;

        /// <summary>
        /// 广告加载失败回调。
        /// </summary>
        /// <remarks>
        /// Callback for advertisement load failure.
        /// </remarks>
        [Preserve] protected Action<string> OnLoadFail;

        /// <summary>
        /// 扩展数据键值对，通过 SetExtraData 存储，Load 时序列化为 JSON 透传到服务端。
        /// </summary>
        /// <remarks>
        /// Extended data key-value pairs stored via SetExtraData, serialized to JSON at Load time for server-side pass-through.
        /// </remarks>
        [Preserve] protected readonly Dictionary<string, string> ExtraData = new Dictionary<string, string>();

        /// <summary>
        /// 使用配置对象初始化广告管理器。
        /// </summary>
        /// <remarks>
        /// Initialize the advertisement manager with a configuration object.
        /// </remarks>
        /// <param name="config">广告配置对象 / Advertisement configuration object</param>
        /// <exception cref="ArgumentNullException">当 <paramref name="config"/> 为 null 时抛出 / Thrown when <paramref name="config"/> is null</exception>
        [Preserve]
        public virtual void Initialize(AdvertisementConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (_delegating)
            {
                return;
            }

            _delegating = true;
            try
            {
                var defaultConfig = config as DefaultAdvertisementConfig;
                Initialize(defaultConfig != null ? defaultConfig.adUnitId : string.Empty, config.isDebug);
            }
            finally
            {
                _delegating = false;
            }
        }

        /// <summary>
        /// 使用广告单元ID初始化广告管理器。
        /// </summary>
        /// <remarks>
        /// Initialize the advertisement manager with an ad unit ID.
        /// </remarks>
        /// <param name="adUnitId">广告单元ID / Ad unit ID</param>
        /// <param name="debug">是否启用调试模式 / Whether to enable debug mode</param>
        [Obsolete("Use Initialize(AdvertisementConfig) instead.")]
        [Preserve]
        public virtual void Initialize(string adUnitId, bool debug = false)
        {
            if (_delegating)
            {
                return;
            }

            _delegating = true;
            try
            {
                var config = CreateDefaultConfig(adUnitId, debug);
                Initialize(config);
            }
            finally
            {
                _delegating = false;
            }
        }

        /// <summary>
        /// 设置广告额外数据。
        /// </summary>
        /// <remarks>
        /// Set extra data for the advertisement.
        /// </remarks>
        /// <param name="key">数据键 / Data key</param>
        /// <param name="value">数据值 / Data value</param>
        [Preserve]
        public virtual void SetExtraData(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            if (value == null)
            {
                ExtraData.Remove(key);
            }
            else
            {
                ExtraData[key] = value;
            }
        }

        /// <summary>
        /// 播放广告。
        /// </summary>
        /// <remarks>
        /// Play the advertisement.
        /// </remarks>
        /// <param name="playResult">播放结果回调，参数表示是否成功播放 / Play result callback, parameter indicates whether the ad was played successfully</param>
        /// <param name="customData">自定义数据 / Custom data</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public abstract void Play(Action<bool> playResult, string customData = null);

        /// <summary>
        /// 使用播放选项播放广告。
        /// </summary>
        /// <remarks>
        /// Play the advertisement with play options.
        /// </remarks>
        /// <param name="option">广告播放选项 / Advertisement play options</param>
        [Preserve]
        public abstract void Play(AdvertisementPlayOption option);

        /// <summary>
        /// 展示广告。
        /// </summary>
        /// <remarks>
        /// Show the advertisement.
        /// </remarks>
        /// <param name="success">展示成功回调，参数为成功信息 / Show success callback, parameter is success information</param>
        /// <param name="fail">展示失败回调，参数为失败原因 / Show failure callback, parameter is failure reason</param>
        /// <param name="onShowResult">展示结果回调，参数表示是否完整观看 / Show result callback, parameter indicates whether the ad was fully watched</param>
        /// <param name="customData">自定义数据 / Custom data</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public abstract void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult, string customData = null);

        /// <summary>
        /// 加载广告资源。
        /// </summary>
        /// <remarks>
        /// Load the advertisement resource.
        /// </remarks>
        /// <param name="success">加载成功回调，参数为成功信息 / Load success callback, parameter is success information</param>
        /// <param name="fail">加载失败回调，参数为失败原因 / Load failure callback, parameter is failure reason</param>
        /// <param name="customData">自定义数据 / Custom data</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public abstract void Load(Action<string> success, Action<string> fail, string customData = null);

        /// <summary>
        /// 触发广告加载成功回调。
        /// </summary>
        /// <remarks>
        /// Trigger the ad load success callback.
        /// </remarks>
        /// <param name="json">成功信息（JSON格式） / Success information in JSON format</param>
        [Preserve]
        protected void LoadSuccess(string json)
        {
            OnLoadSuccess?.Invoke(json);
        }

        /// <summary>
        /// 触发广告加载失败回调。
        /// </summary>
        /// <remarks>
        /// Trigger the ad load failure callback.
        /// </remarks>
        /// <param name="json">失败信息（JSON格式） / Failure information in JSON format</param>
        [Preserve]
        protected void LoadFail(string json)
        {
            OnLoadFail?.Invoke(json);
        }

        /// <summary>
        /// 将扩展数据字典序列化为 JSON 字符串，用于透传到广告 SDK 服务端验证回调。
        /// </summary>
        /// <remarks>
        /// Serializes the extended data dictionary to a JSON string for pass-through to the ad SDK server-side verification callback.
        /// </remarks>
        /// <returns>序列化后的 JSON 字符串；如果无扩展数据则返回 null / Serialized JSON string; null if no extended data</returns>
        [Preserve]
        protected string SerializeExtraData()
        {
            if (ExtraData.Count == 0)
            {
                return null;
            }

            return Utility.Json.ToJson(ExtraData);
        }

        /// <summary>
        /// 获取指定键的扩展数据值。
        /// </summary>
        /// <remarks>
        /// Gets the extended data value for the specified key.
        /// </remarks>
        /// <param name="key">数据键 / Data key</param>
        /// <returns>指定键对应的值；如果键不存在则返回 null / Value for the specified key; null if the key does not exist</returns>
        [Preserve]
        protected string GetExtraDataValue(string key)
        {
            ExtraData.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// 根据广告单元ID和调试模式创建默认配置对象。
        /// </summary>
        /// <remarks>
        /// Creates a default configuration object from the ad unit ID and debug mode.
        /// </remarks>
        /// <param name="adUnitId">广告单元ID / Ad unit ID</param>
        /// <param name="debug">是否启用调试模式 / Whether to enable debug mode</param>
        /// <returns>默认广告配置对象 / Default advertisement configuration object</returns>
        protected virtual AdvertisementConfig CreateDefaultConfig(string adUnitId, bool debug)
        {
            var config = new DefaultAdvertisementConfig();
            config.adUnitId = adUnitId;
            config.isDebug = debug;
            return config;
        }

        /// <summary>
        /// 默认广告配置，内部用于兼容旧版字符串初始化 API。
        /// </summary>
        /// <remarks>
        /// Default advertisement configuration, used internally for backward compatibility with legacy string-based initialization API.
        /// </remarks>
        [Serializable]
        private class DefaultAdvertisementConfig : AdvertisementConfig
        {
            /// <summary>
            /// 广告单元ID。
            /// </summary>
            /// <remarks>
            /// Ad unit ID.
            /// </remarks>
            public string adUnitId;
        }
    }
}