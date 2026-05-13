using System;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告配置基类，用于承载广告管理器初始化所需的参数。
    /// </summary>
    /// <remarks>
    /// Base class for advertisement configuration, carrying parameters needed by advertisement manager initialization.
    /// </remarks>
    [Preserve]
    [Serializable]
    public abstract class AdvertisementConfig
    {
        /// <summary>
        /// 是否启用调试模式。
        /// </summary>
        /// <remarks>
        /// Whether to enable debug mode.
        /// </remarks>
        [Preserve]
        public bool isDebug;
    }
}
