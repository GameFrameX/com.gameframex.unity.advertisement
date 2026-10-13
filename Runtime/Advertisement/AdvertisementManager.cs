// ==========================================================================================
//   GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//   GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//   均受中华人民共和国及相关国际法律法规保护。
//   are protected by the laws of the People's Republic of China and relevant international regulations.
//   使用本项目须严格遵守相关法律法规及开源许可证之规定。
//   Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//   本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//   This project is dual-licensed under the MIT License and Apache License 2.0,
//   完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//   please refer to the LICENSE file in the root of the source tree for the full license text.
//   禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//   It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//   侵犯他人合法权益等法律法规所禁止的行为！
//   or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//   因基于本项目二次开发所产生的一切法律纠纷与责任，
//   Any disputes or liabilities arising from secondary development based on this project
//   本项目组织及贡献者概不承担。
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
using GameFrameX.Runtime;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告默认管理器，主包模块槽位的默认实现。
    /// </summary>
    /// <remarks>
    /// Default advertisement manager occupying the module slot of the main package.
    /// 模块级解析轴（GameFrameXRuntimeManagerResolver）的落点：<c>IAdvertisementManager</c> 的模块注册收敛到本主包类型，
    /// 渠道实现不再各自直接占用模块槽位。实际的渠道调用经 <see cref="AdvertisementRegistry.ResolveManager"/> 互斥解析转发；
    /// 框架关闭时把 Shutdown 一并转发给解析到的渠道管理器（如 csj 的原生 Bridge 释放）。
    /// </remarks>
    [Preserve]
    public sealed class AdvertisementManager : BaseAdvertisementManager, IShutdownableModule
    {
        private bool _noChannelWarned;

        /// <summary>
        /// 使用配置对象初始化广告管理器，转发给注册表互斥解析到的渠道管理器。
        /// </summary>
        /// <param name="config">广告配置对象 / Advertisement configuration object</param>
        [Preserve]
        public override void Initialize(AdvertisementConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            ResolveChannel()?.Initialize(config);
        }

        /// <summary>
        /// 使用广告单元ID初始化广告管理器，转发给注册表互斥解析到的渠道管理器。
        /// </summary>
        /// <param name="adUnitId">广告单元ID / Ad unit ID</param>
        /// <param name="debug">是否启用调试模式 / Whether to enable debug mode</param>
        [Obsolete("Use Initialize(AdvertisementConfig) instead.")]
        [Preserve]
        public override void Initialize(string adUnitId, bool debug = false)
        {
#pragma warning disable CS0618
            ResolveChannel()?.Initialize(adUnitId, debug);
#pragma warning restore CS0618
        }

        /// <summary>
        /// 设置广告额外数据，转发给注册表互斥解析到的渠道管理器。
        /// </summary>
        /// <param name="key">数据键 / Data key</param>
        /// <param name="value">数据值 / Data value</param>
        [Preserve]
        public override void SetExtraData(string key, string value)
        {
            ResolveChannel()?.SetExtraData(key, value);
        }

        /// <summary>
        /// 播放广告，转发给注册表互斥解析到的渠道管理器。
        /// </summary>
        /// <param name="playResult">播放结果回调 / Play result callback</param>
        /// <param name="customData">自定义数据 / Custom data</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public override void Play(Action<bool> playResult, string customData = null)
        {
#pragma warning disable CS0618
            ResolveChannel()?.Play(playResult, customData);
#pragma warning restore CS0618
        }

        /// <summary>
        /// 使用播放选项播放广告，转发给注册表互斥解析到的渠道管理器。
        /// </summary>
        /// <param name="option">广告播放选项 / Advertisement play options</param>
        [Preserve]
        public override void Play(AdvertisementPlayOption option)
        {
            ResolveChannel()?.Play(option);
        }

        /// <summary>
        /// 展示广告，转发给注册表互斥解析到的渠道管理器。
        /// </summary>
        /// <param name="success">展示成功回调 / Show success callback</param>
        /// <param name="fail">展示失败回调 / Show failure callback</param>
        /// <param name="onShowResult">展示结果回调 / Show result callback</param>
        /// <param name="customData">自定义数据 / Custom data</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public override void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult, string customData = null)
        {
#pragma warning disable CS0618
            ResolveChannel()?.Show(success, fail, onShowResult, customData);
#pragma warning restore CS0618
        }

        /// <summary>
        /// 加载广告资源，转发给注册表互斥解析到的渠道管理器。
        /// </summary>
        /// <param name="success">加载成功回调 / Load success callback</param>
        /// <param name="fail">加载失败回调 / Load failure callback</param>
        /// <param name="customData">自定义数据 / Custom data</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public override void Load(Action<string> success, Action<string> fail, string customData = null)
        {
#pragma warning disable CS0618
            ResolveChannel()?.Load(success, fail, customData);
#pragma warning restore CS0618
        }

        /// <summary>
        /// 关闭模块：把关闭时机转发给注册表互斥解析到的渠道管理器（渠道自身实现 <c>IShutdownableModule</c> 时生效）。
        /// </summary>
        [Preserve]
        public void Shutdown()
        {
            (AdvertisementRegistry.ResolveManager() as IShutdownableModule)?.Shutdown();
        }

        /// <summary>
        /// 互斥解析当前渠道管理器；无注册渠道时告警（每次会话只告警一次）并返回 null。
        /// </summary>
        /// <returns>渠道管理器；无可用渠道返回null</returns>
        private IAdvertisementManager ResolveChannel()
        {
            var manager = AdvertisementRegistry.ResolveManager();
            if (manager == null && !_noChannelWarned)
            {
                _noChannelWarned = true;
                Log.Warning("No advertisement channel registered. Install a channel package (csj / topon / mini-game) or register one via AdvertisementRegistry.");
            }

            return manager;
        }
    }
}
