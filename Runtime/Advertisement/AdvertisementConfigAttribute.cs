using System;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 用于标记广告管理器对应的配置类型，供 Inspector 自动反射绘制配置字段。
    /// </summary>
    /// <remarks>
    /// Used to mark the configuration type associated with an advertisement manager, enabling the Inspector to automatically draw config fields via reflection.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AdvertisementConfigAttribute : Attribute
    {
        /// <summary>
        /// 获取广告管理器关联的配置类型。
        /// </summary>
        /// <remarks>
        /// Gets the configuration type associated with the advertisement manager.
        /// </remarks>
        /// <value>配置类型 / Configuration type</value>
        public Type ConfigType { get; }

        /// <summary>
        /// 初始化 <see cref="AdvertisementConfigAttribute"/> 实例。
        /// </summary>
        /// <remarks>
        /// Initializes a new instance of the <see cref="AdvertisementConfigAttribute"/> class.
        /// </remarks>
        /// <param name="configType">广告管理器使用的配置类型 / Configuration type used by the advertisement manager</param>
        public AdvertisementConfigAttribute(Type configType)
        {
            ConfigType = configType;
        }
    }
}
