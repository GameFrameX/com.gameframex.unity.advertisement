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
using GameFrameX.Runtime;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告组件，提供广告初始化、播放和加载的 Unity 组件封装。
    /// </summary>
    /// <remarks>
    /// Advertisement component, providing a Unity component wrapper for ad initialization, playback, and loading.
    /// </remarks>
    [DisallowMultipleComponent]
    [AddComponentMenu("GameFrameX/Advertisement")]
    public class AdvertisementComponent : GameFrameworkComponent
    {
        [SerializeField] private string m_adUnitIdAndroid = string.Empty;

        [SerializeField] private string m_adUnitIdiOS = string.Empty;

        [SerializeField] private string m_adUnitIdWebGL = string.Empty;

        [SerializeField] private string m_adUnitIdWebGLWeChat = string.Empty;

        [SerializeField] private string m_adUnitIdWebGLDouYin = string.Empty;

        [SerializeField] private string m_adUnitIdWebGLKuaiShou = string.Empty;

        [SerializeField] private bool m_debug = false;

        [SerializeReference, SerializeField] private AdvertisementConfig m_config = null;

        /// <summary>
        /// 获取当前平台的广告位ID。
        /// </summary>
        /// <remarks>
        /// Gets the ad unit ID for the current platform.
        /// </remarks>
        /// <value>广告位ID / Ad unit ID</value>
        public string AdUnitId { get; private set; }

        /// <summary>
        /// 获取或设置广告配置对象。
        /// </summary>
        /// <remarks>
        /// Gets or sets the advertisement configuration object.
        /// </remarks>
        /// <value>广告配置对象 / Advertisement configuration object</value>
        public AdvertisementConfig Config
        {
            get => m_config;
            set => m_config = value;
        }

        private IAdvertisementManager _advertisementManager;

        protected override void Awake()
        {
            ImplementationComponentType = Utility.Assembly.GetType(componentType);
            InterfaceComponentType = typeof(IAdvertisementManager);
            base.Awake();
            _advertisementManager = GameFrameworkEntry.GetModule<IAdvertisementManager>();
            if (_advertisementManager == null)
            {
                Log.Fatal("Advertisement manager is invalid.");
                return;
            }

            AdUnitId =
#if UNITY_WEBGL
#if ENABLE_WECHAT_MINI_GAME
        m_adUnitIdWebGLWeChat
#elif ENABLE_DOUYIN_MINI_GAME
                m_adUnitIdWebGLDouYin
#elif ENABLE_KUAISHOU_MINI_GAME
                m_adUnitIdWebGLKuaiShou
#else
                m_adUnitIdWebGL
#endif
#elif UNITY_ANDROID
                m_adUnitIdAndroid
#elif UNITY_IOS
                m_adUnitIdiOS
#else
                string.Empty
#endif
                ;
        }

        private void Start()
        {
            if (m_config != null)
            {
                _advertisementManager.Initialize(m_config);
            }
        }

        /// <summary>
        /// 使用配置对象初始化广告组件。
        /// </summary>
        /// <remarks>
        /// Initialize the advertisement component with a configuration object.
        /// </remarks>
        /// <param name="config">广告配置对象 / Advertisement configuration object</param>
        [Preserve]
        public void Initialize(AdvertisementConfig config)
        {
            m_config = config;
            _advertisementManager.Initialize(config);
        }

        /// <summary>
        /// 使用广告位ID初始化广告组件。
        /// </summary>
        /// <remarks>
        /// Initialize the advertisement component with an ad unit ID.
        /// </remarks>
        /// <param name="adUnitId">广告位ID / Ad unit ID</param>
        /// <param name="isDebug">是否启用调试模式 / Whether to enable debug mode</param>
        [Obsolete("Use Initialize(AdvertisementConfig) instead.")]
        [Preserve]
        public void Initialize(string adUnitId, bool isDebug = false)
        {
            AdUnitId = adUnitId;
#pragma warning disable CS0618
            _advertisementManager.Initialize(AdUnitId, isDebug);
#pragma warning restore CS0618
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
        public void SetExtraData(string key, string value)
        {
            _advertisementManager.SetExtraData(key, value);
        }

        /// <summary>
        /// 展示广告。
        /// </summary>
        /// <remarks>
        /// Show the advertisement.
        /// </remarks>
        /// <param name="success">展示成功回调，参数为成功信息 / Show success callback, parameter is success information</param>
        /// <param name="fail">展示失败回调，参数为失败原因 / Show failure callback, parameter is failure reason</param>
        /// <param name="onShowResult">展示结果回调，参数表示是否完整观看 / Show result callback, parameter indicates whether the ad was fully watched</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult)
        {
#pragma warning disable CS0618
            _advertisementManager.Show(success, fail, onShowResult);
#pragma warning restore CS0618
        }

        /// <summary>
        /// 播放广告。
        /// </summary>
        /// <remarks>
        /// Play the advertisement.
        /// </remarks>
        /// <param name="onShowResult">播放结果回调，参数表示是否成功播放 / Play result callback, parameter indicates whether the ad was played successfully</param>
        /// <param name="customData">自定义数据 / Custom data</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public void Play(Action<bool> onShowResult, string customData = null)
        {
            _advertisementManager.Play(onShowResult, customData);
        }

        /// <summary>
        /// 使用播放选项播放广告。
        /// </summary>
        /// <remarks>
        /// Play the advertisement with play options.
        /// </remarks>
        /// <param name="option">广告播放选项 / Advertisement play options</param>
        [Preserve]
        public void Play(AdvertisementPlayOption option)
        {
            _advertisementManager.Play(option);
        }

        /// <summary>
        /// 加载广告资源。
        /// </summary>
        /// <remarks>
        /// Load the advertisement resource.
        /// </remarks>
        /// <param name="success">加载成功回调，参数为成功信息 / Load success callback, parameter is success information</param>
        /// <param name="fail">加载失败回调，参数为失败原因 / Load failure callback, parameter is failure reason</param>
        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public void Load(Action<string> success, Action<string> fail)
        {
#pragma warning disable CS0618
            _advertisementManager.Load(success, fail);
#pragma warning restore CS0618
        }
    }
}
