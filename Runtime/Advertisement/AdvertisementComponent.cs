using System;
using GameFrameX.Runtime;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
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

        public string AdUnitId { get; private set; }

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

        [Preserve]
        public void Initialize(AdvertisementConfig config)
        {
            m_config = config;
            _advertisementManager.Initialize(config);
        }

        [Obsolete("Use Initialize(AdvertisementConfig) instead.")]
        [Preserve]
        public void Initialize(string adUnitId, bool isDebug = false)
        {
            AdUnitId = adUnitId;
#pragma warning disable CS0618
            _advertisementManager.Initialize(AdUnitId, isDebug);
#pragma warning restore CS0618
        }

        [Preserve]
        public void SetExtraData(string key, string value)
        {
            _advertisementManager.SetExtraData(key, value);
        }

        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult)
        {
#pragma warning disable CS0618
            _advertisementManager.Show(success, fail, onShowResult);
#pragma warning restore CS0618
        }

        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public void Play(Action<bool> onShowResult, string customData = null)
        {
            _advertisementManager.Play(onShowResult, customData);
        }

        [Preserve]
        public void Play(AdvertisementPlayOption option)
        {
            _advertisementManager.Play(option);
        }

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
