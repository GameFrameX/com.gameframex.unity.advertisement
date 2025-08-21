using System;
using GameFrameX.Runtime;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告管理器基类
    /// </summary>
    public abstract class BaseAdvertisementManager : GameFrameworkModule, IAdvertisementManager
    {
        /// <summary>
        /// 更新方法
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位</param>
        protected override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭时的清理方法
        /// </summary>
        protected override void Shutdown()
        {
        }

        /// <summary>
        /// 广告展示结果回调
        /// </summary>
        protected Action<bool> OnShowResult;

        /// <summary>
        /// 广告加载成功回调
        /// </summary>
        protected Action<string> OnLoadSuccess;

        /// <summary>
        /// 广告加载失败回调
        /// </summary>
        protected Action<string> OnLoadFail;

        /// <summary>
        /// 初始化广告管理器
        /// </summary>
        /// <param name="adUnitId">广告单元ID</param>
        /// <param name="debug">是否开启调试模式</param>
        public abstract void Initialize(string adUnitId, bool debug = false);

        /// <summary>
        /// 设置额外数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public virtual void SetExtraData(string key, string value)
        {
        }

        /// <summary>
        /// 展示广告
        /// </summary>
        /// <param name="success">展示成功回调</param>
        /// <param name="fail">展示失败回调</param>
        /// <param name="onShowResult">展示结果回调</param>
        /// <param name="customData">自定义数据</param>
        public abstract void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult, string customData = null);

        /// <summary>
        /// 加载广告
        /// </summary>
        /// <param name="success">加载成功回调</param>
        /// <param name="fail">加载失败回调</param>
        public abstract void Load(Action<string> success, Action<string> fail);

        /// <summary>
        /// 处理广告加载成功
        /// </summary>
        /// <param name="json">成功信息（JSON格式）</param>
        protected void LoadSuccess(string json)
        {
            OnLoadSuccess?.Invoke(json);
        }

        /// <summary>
        /// 处理广告加载失败
        /// </summary>
        /// <param name="json">失败信息（JSON格式）</param>
        protected void LoadFail(string json)
        {
            OnLoadFail?.Invoke(json);
        }
    }
}