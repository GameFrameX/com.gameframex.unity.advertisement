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

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告渠道注册描述
    /// </summary>
    /// <remarks>
    /// 由渠道包在启动时通过 <see cref="AdvertisementRegistry.Register(AdvertisementChannelRegistration)"/> 显式注册，
    /// 工厂委托在首次解析时才实例化渠道管理器，并在闭包内注入强类型 <see cref="AdvertisementConfig"/> 子类（appId / adUnitId 等）。
    /// 未安装渠道包时其代码被裁剪，注册不会发生（引用即注册）。
    /// </remarks>
    [UnityEngine.Scripting.Preserve]
    public sealed class AdvertisementChannelRegistration
    {
        /// <summary>
        /// 初始化广告渠道注册描述
        /// </summary>
        /// <param name="channelId">渠道唯一标识</param>
        /// <param name="displayName">渠道展示名称（用于 UI 展示）</param>
        /// <param name="factory">渠道管理器工厂，编译期类型安全，首次解析时调用并注入强类型配置</param>
        [UnityEngine.Scripting.Preserve]
        public AdvertisementChannelRegistration(string channelId, string displayName, Func<IAdvertisementManager> factory)
        {
            if (string.IsNullOrEmpty(channelId))
            {
                throw new ArgumentNullException(nameof(channelId));
            }

            ChannelId = channelId;
            DisplayName = displayName;
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// 获取渠道唯一标识
        /// </summary>
        public string ChannelId { get; }

        /// <summary>
        /// 获取渠道展示名称
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// 获取渠道管理器工厂
        /// </summary>
        public Func<IAdvertisementManager> Factory { get; }
    }
}
