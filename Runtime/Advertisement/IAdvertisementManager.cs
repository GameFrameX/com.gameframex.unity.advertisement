using System;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
    [Preserve]
    public interface IAdvertisementManager
    {
        [Preserve]
        void Initialize(AdvertisementConfig config);

        [Obsolete("Use Initialize(AdvertisementConfig) instead.")]
        [Preserve]
        void Initialize(string adUnitId, bool isDebug = false);

        [Preserve]
        void SetExtraData(string key, string value);

        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        void Play(Action<bool> playResult, string customData = null);

        [Preserve]
        void Play(AdvertisementPlayOption option);

        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult, string customData = null);

        [Obsolete("Use Play(AdvertisementPlayOption) instead.")]
        [Preserve]
        void Load(Action<string> success, Action<string> fail, string customData = null);
    }
}
