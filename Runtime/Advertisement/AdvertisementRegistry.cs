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

using System.Collections.Generic;
using GameFrameX.Runtime;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告渠道注册表
    /// </summary>
    /// <remarks>
    /// 渠道包通过自身的 Bootstrap（RuntimeInitializeOnLoadMethod）在启动时显式注册，未被引用的渠道包代码会被裁剪，天然满足“引用即注册”。
    /// 互斥解析语义：同一时刻只解析出一个渠道管理器——无显式渠道标识时按注册顺序取首个（宏墙保证小游戏渠道天然互斥；
    /// csj / topon 同装时需通过显式渠道标识覆盖），这与支付域 <c>PaymentChannelRegistry</c> 的“一笔单 → 一个渠道”参数路由、
    /// 统计域 <c>GameAnalyticsRegistry</c> 的事件广播（fan-out 到全部渠道）语义均不同。
    /// 显式路由覆盖：<see cref="ResolveManager"/> 携带渠道标识时按注册表路由到指定渠道（未注册返回 null），用于 csj / topon 人工选型场景。
    /// 注册表在 SubsystemRegistration 阶段自动清空（早于各渠道的 BeforeSceneLoad 注册），关闭 Domain Reload 时不会产生重复注册。
    /// 注册表实例化的渠道管理器不参与 GameFrameworkEntry 的模块 Update/Shutdown 生命周期；
    /// 模块槽位由主包默认实现 <see cref="AdvertisementManager"/> 占据，并把调用转发给注册表解析结果（含 Shutdown 转发）。
    /// </remarks>
    [UnityEngine.Scripting.Preserve]
    public static class AdvertisementRegistry
    {
        private static readonly Dictionary<string, AdvertisementChannelRegistration> Registrations = new Dictionary<string, AdvertisementChannelRegistration>(8);
        private static readonly Dictionary<string, IAdvertisementManager> Managers = new Dictionary<string, IAdvertisementManager>(8);

        /// <summary>
        /// 重置广告渠道注册表
        /// </summary>
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        [UnityEngine.Scripting.Preserve]
        public static void Reset()
        {
            Registrations.Clear();
            Managers.Clear();
        }

        /// <summary>
        /// 注册广告渠道（同标识重复注册时保留首个并告警）
        /// </summary>
        /// <param name="registration">渠道注册描述</param>
        [UnityEngine.Scripting.Preserve]
        public static void Register(AdvertisementChannelRegistration registration)
        {
            if (registration == null)
            {
                return;
            }

            if (Registrations.ContainsKey(registration.ChannelId))
            {
                Log.Warning("Advertisement channel '{0}' is already registered. The duplicate registration is ignored.", registration.ChannelId);
                return;
            }

            Registrations.Add(registration.ChannelId, registration);
        }

        /// <summary>
        /// 注销广告渠道
        /// </summary>
        /// <param name="channelId">渠道唯一标识</param>
        [UnityEngine.Scripting.Preserve]
        public static void Unregister(string channelId)
        {
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }

            Registrations.Remove(channelId);
            Managers.Remove(channelId);
        }

        /// <summary>
        /// 渠道是否已注册
        /// </summary>
        /// <param name="channelId">渠道唯一标识</param>
        /// <returns>已注册返回true，否则返回false</returns>
        [UnityEngine.Scripting.Preserve]
        public static bool Contains(string channelId)
        {
            return !string.IsNullOrEmpty(channelId) && Registrations.ContainsKey(channelId);
        }

        /// <summary>
        /// 获取渠道管理器（首次获取时通过工厂实例化并缓存）
        /// </summary>
        /// <param name="channelId">渠道唯一标识</param>
        /// <returns>渠道管理器；未注册返回null</returns>
        [UnityEngine.Scripting.Preserve]
        public static IAdvertisementManager GetManager(string channelId)
        {
            if (string.IsNullOrEmpty(channelId) || !Registrations.TryGetValue(channelId, out var registration))
            {
                return null;
            }

            if (!Managers.TryGetValue(channelId, out var manager))
            {
                manager = registration.Factory();
                Managers[channelId] = manager;
            }

            return manager;
        }

        /// <summary>
        /// 获取全部已注册渠道的管理器（懒实例化并缓存，返回顺序与注册顺序一致）
        /// </summary>
        /// <returns>渠道管理器列表；无注册渠道时为空列表</returns>
        [UnityEngine.Scripting.Preserve]
        public static IReadOnlyList<IAdvertisementManager> GetAllManagers()
        {
            var managers = new List<IAdvertisementManager>(Registrations.Count);
            foreach (var registration in Registrations.Values)
            {
                var manager = GetManager(registration.ChannelId);
                if (manager != null)
                {
                    managers.Add(manager);
                }
            }

            return managers;
        }

        /// <summary>
        /// 解析当前使用的广告渠道管理器
        /// </summary>
        /// <remarks>
        /// 互斥解析：未指定渠道标识时按注册顺序取首个注册渠道（注册顺序即优先级；小游戏渠道由宏墙保证天然互斥），
        /// 多渠道并存且未显式指定时输出告警；显式渠道标识时按注册表路由到指定渠道（未注册返回 null）。
        /// 解析结果懒实例化并缓存，重复调用返回同一实例。
        /// </remarks>
        /// <param name="channelId">渠道唯一标识，为空时走互斥解析</param>
        /// <returns>渠道管理器；无可用渠道或未注册返回null</returns>
        [UnityEngine.Scripting.Preserve]
        public static IAdvertisementManager ResolveManager(string channelId = null)
        {
            if (!string.IsNullOrEmpty(channelId))
            {
                return GetManager(channelId);
            }

            foreach (var registration in Registrations.Values)
            {
                if (Registrations.Count > 1)
                {
                    Log.Warning("Advertisement has {0} registered channels but no explicit channel id given. Resolving by registration priority: '{1}'.", Registrations.Count, registration.ChannelId);
                }

                return GetManager(registration.ChannelId);
            }

            return null;
        }
    }
}
