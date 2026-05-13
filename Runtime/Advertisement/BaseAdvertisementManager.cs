using System;
using GameFrameX.Runtime;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
    [Preserve]
    public abstract class BaseAdvertisementManager : GameFrameworkModule, IAdvertisementManager
    {
        private bool _delegating;

        protected override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        protected override void Shutdown()
        {
        }

        [Preserve] protected Action<bool> OnShowResult;

        [Preserve] protected Action<string> OnLoadSuccess;

        [Preserve] protected Action<string> OnLoadFail;

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

        [Obsolete("Use Initialize(AdvertisementConfig) instead.")]
        [Preserve]
        public virtual void Initialize(string adUnitId, bool debug = false)
        {
            if (_delegating) return;
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

        [Preserve]
        public virtual void SetExtraData(string key, string value)
        {
        }

        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public abstract void Play(Action<bool> playResult, string customData = null);

        [Preserve]
        public abstract void Play(AdvertisementPlayOption option);

        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public abstract void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult, string customData = null);

        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        public abstract void Load(Action<string> success, Action<string> fail, string customData = null);

        [Preserve]
        protected void LoadSuccess(string json)
        {
            OnLoadSuccess?.Invoke(json);
        }

        [Preserve]
        protected void LoadFail(string json)
        {
            OnLoadFail?.Invoke(json);
        }

        protected virtual AdvertisementConfig CreateDefaultConfig(string adUnitId, bool debug)
        {
            var config = new DefaultAdvertisementConfig();
            config.adUnitId = adUnitId;
            config.isDebug = debug;
            return config;
        }

        [Serializable]
        private class DefaultAdvertisementConfig : AdvertisementConfig
        {
            public string adUnitId;
        }
    }
}
