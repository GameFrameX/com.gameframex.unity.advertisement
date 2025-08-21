using System;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告管理器接口
    /// </summary>
    public interface IAdvertisementManager
    {
        /// <summary>
        /// 初始化广告管理器
        /// </summary>
        /// <param name="adUnitId">广告单元ID</param>
        /// <param name="isDebug">是否是调试模式</param>
        void Initialize(string adUnitId, bool isDebug = false);

        /// <summary>
        /// 设置额外的广告数据
        /// </summary>
        /// <param name="key">数据键</param>
        /// <param name="value">数据值</param>
        void SetExtraData(string key, string value);

        /// <summary>
        /// 展示广告
        /// </summary>
        /// <param name="success">展示成功回调,参数为成功信息</param>
        /// <param name="fail">展示失败回调,参数为失败原因</param>
        /// <param name="onShowResult">展示结果回调,true表示用户完整观看广告,false表示用户跳过广告</param>
        /// <param name="customData">自定义数据</param>
        void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult, string customData = null);

        /// <summary>
        /// 加载广告
        /// </summary>
        /// <param name="success">加载成功回调,参数为成功信息</param>
        /// <param name="fail">加载失败回调,参数为失败原因</param>
        /// <param name="customData">自定义数据</param>
        void Load(Action<string> success, Action<string> fail, string customData = null);
    }
}