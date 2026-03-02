using System;
using GameFrameX.Runtime;
using UnityEngine;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// Advertisement 组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Advertisement")]
    public class AdvertisementComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 广告位ID Android
        /// </summary>
        [SerializeField] private string m_adUnitIdAndroid = string.Empty;

        /// <summary>
        /// 广告位ID iOS
        /// </summary>
        [SerializeField] private string m_adUnitIdiOS = string.Empty;

        /// <summary>
        /// 广告位ID WebGL
        /// </summary>
        [SerializeField] private string m_adUnitIdWebGL = string.Empty;

        /// <summary>
        /// 广告位ID WebGL
        /// </summary>
        [SerializeField] private string m_adUnitIdWebGLWeChat = string.Empty;

        /// <summary>
        /// 广告位ID WebGL
        /// </summary>
        [SerializeField] private string m_adUnitIdWebGLDouYin = string.Empty;

        /// <summary>
        /// 是否是测试模式
        /// </summary>
        [SerializeField] private bool m_debug = false;

        /// <summary>
        /// 广告位ID
        /// </summary>
        public string AdUnitId { get; private set; }

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
            _advertisementManager.Initialize(AdUnitId, m_debug);
        }

        /// <summary>
        /// 初始化广告
        /// </summary>
        /// <param name="adUnitId">广告位ID</param>
        /// <param name="isDebug">是否是测试模式</param>
        public void Initialize(string adUnitId, bool isDebug = false)
        {
            AdUnitId = adUnitId;
            _advertisementManager.Initialize(AdUnitId, isDebug);
        }

        /// <summary>
        /// 设置广告额外数据
        /// </summary>
        /// <param name="key">数据键</param>
        /// <param name="value">数据值</param>
        /// <remarks>
        /// 用于在展示广告前设置一些额外的参数数据，这些数据可能会被广告SDK使用
        /// </remarks>
        public void SetExtraData(string key, string value)
        {
            _advertisementManager.SetExtraData(key, value);
        }

        /// <summary>
        /// 展示广告
        /// </summary>
        /// <param name="success">展示成功回调</param>
        /// <param name="fail">展示失败回调</param>
        /// <param name="onShowResult">展示成功后广告关闭回调。参数值为true表示广告应该发放奖励，为false表示没有完整播放完广告</param>
        /// <remarks>
        /// 在调用此方法前，请确保已经通过Load方法预加载了广告
        /// success回调会返回广告展示相关的信息字符串
        /// fail回调会返回失败原因字符串
        /// </remarks>
        public void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult)
        {
            _advertisementManager.Show(success, fail, onShowResult);
        }

        /// <summary>
        /// 展示广告
        /// </summary>
        /// <param name="onShowResult">展示成功后广告关闭回调。参数值为true表示广告应该发放奖励，为false表示没有完整播放完广告</param>
        /// <param name="customData">自定义数据</param>
        /// <remarks>
        /// 在调用此方法前，请确保已经通过Load方法预加载了广告
        /// onShowResult回调的布尔值表示广告是否完整观看（true为完整观看并可获得奖励，false为未完整观看）
        /// </remarks>
        public void Play(Action<bool> onShowResult, string customData = null)
        {
            _advertisementManager.Play(onShowResult, customData);
        }

        /// <summary>
        /// 加载广告
        /// </summary>
        /// <param name="success">加载成功回调</param>
        /// <param name="fail">加载失败回调</param>
        /// <remarks>
        /// 建议在展示广告前预先调用此方法加载广告资源
        /// success回调会返回广告加载成功的相关信息字符串
        /// fail回调会返回加载失败的原因字符串
        /// </remarks>
        public void Load(Action<string> success, Action<string> fail)
        {
            _advertisementManager.Load(success, fail);
        }
    }
}