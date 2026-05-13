using System;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告播放选项，用于统一承载播放时的回调与扩展数据。
    /// </summary>
    /// <remarks>
    /// Advertisement play options, used to unify callbacks and extended data during ad playback.
    /// </remarks>
    [Preserve]
    [Serializable]
    public class AdvertisementPlayOption
    {
        /// <summary>
        /// 广告播放成功（如奖励验证通过）回调，参数为扩展数据。
        /// </summary>
        /// <remarks>
        /// Callback invoked when the ad plays successfully (e.g., reward verification passed), with extended data as the parameter.
        /// </remarks>
        [Preserve] public Action<string> onSuccess;

        /// <summary>
        /// 广告播放失败回调，参数为错误信息。
        /// </summary>
        /// <remarks>
        /// Callback invoked when the ad fails to play, with error information as the parameter.
        /// </remarks>
        [Preserve] public Action<string> onFail;

        /// <summary>
        /// 广告展示结果回调，参数表示是否成功展示。
        /// </summary>
        /// <remarks>
        /// Callback invoked with the ad show result, where the parameter indicates whether the ad was successfully displayed.
        /// </remarks>
        [Preserve] public Action<bool> onShowResult;

        /// <summary>
        /// 扩展数据，会透传到服务端奖励验证回调。
        /// </summary>
        /// <remarks>
        /// Extended data that is passed through to the server-side reward verification callback.
        /// </remarks>
        [Preserve] public string extraData;
    }
}
